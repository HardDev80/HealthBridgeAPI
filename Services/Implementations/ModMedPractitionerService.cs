using HealthBridgeAPI.Data;
using HealthBridgeAPI.Models.DTOs;
using HealthBridgeAPI.Models.Entities;
using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HealthBridgeAPI.Services.Implementations
{
    public partial class ModMedPractitionerService : IModMedPractitionerService
    {
        private readonly IModMedRepository _modMedRepository;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public ModMedPractitionerService(
            IModMedRepository modMedRepository,
            IConfiguration configuration,
            AppDbContext db)
        {
            _modMedRepository = modMedRepository;
            _configuration = configuration;
            _db = db;
        }

        
        public async Task<string> GetAllPractitionerList()
        {
            var endpoint = "Practitioner";
            var token = _configuration["ModMedSettings:AccessToken"];
            return await _modMedRepository.GetAsync(endpoint, token);
        }

        public async Task<List<Practitioner>> GetAllPractitionersPaginatedAsync()
        {
            var token = _configuration["ModMedSettings:AccessToken"];

            int page = 1;
            var all = new List<Practitioner>();

            while (true)
            {
                string endpoint = $"Practitioner?type=ref&active=true&page={page}";
                string json = await _modMedRepository.GetAsync(endpoint, token);

                JsonNode? root = JsonNode.Parse(json);
                if (root == null || root["entry"] is not JsonArray entries)
                    break;

                foreach (var entry in entries)
                {
                    var resource = entry?["resource"];
                    if (resource == null) continue;

                    // -------------------------
                    // ACTIVE (solo activos)
                    // -------------------------
                    bool active = resource["active"]?.GetValue<bool>() ?? false;
                    if (!active) continue;

                    // -------------------------
                    // PRACTITIONER ID (ref|101496 → 101496)
                    // -------------------------
                    long identifier = 0;
                    string? rawId = resource["id"]?.ToString();

                    if (!string.IsNullOrWhiteSpace(rawId) && rawId.Contains("|"))
                    {
                        var parts = rawId.Split('|');
                        long.TryParse(parts.Last(), out identifier);
                    }

                    // -------------------------
                    // NAME (family + given)
                    // -------------------------
                    string? family = null;
                    string? given = null;

                    if (resource["name"] is JsonArray nameArray)
                    {
                        var nameObj = nameArray.FirstOrDefault();
                        family = nameObj?["family"]?.ToString();

                        if (nameObj?["given"] is JsonArray givenArray)
                            given = givenArray.FirstOrDefault()?.ToString();
                    }

                    // -------------------------
                    // SPECIALTIES (variass)
                    // qualification[] → code.coding[] → display
                    // -------------------------
                    string? specialties = null;

                    if (resource["qualification"] is JsonArray qualArray)
                    {
                        var specialtyList = new List<string>();

                        foreach (var qual in qualArray)
                        {
                            var codingArray = qual?["code"]?["coding"] as JsonArray;
                            if (codingArray == null) continue;

                            foreach (var cod in codingArray)
                            {
                                var display = cod?["display"]?.ToString();
                                if (!string.IsNullOrWhiteSpace(display))
                                    specialtyList.Add(display);
                            }
                        }

                        if (specialtyList.Count > 0)
                            specialties = string.Join(", ", specialtyList.Distinct());
                    }

                    // -------------------------
                    // MAPEO AL MODELO EXISTENTE
                    // -------------------------
                    var p = new Practitioner
                    {
                        PractitionerIdentifier = identifier,
                        PractitionerStatus = active,
                        PractitionerName = given,
                        PractitionerLastName = family,
                        PractitionerSpecialty = specialties,
                        PractitionerPhotoRef = null,
                        PractitionerProvider = null,
                        PractitionerOnLine = null,
                        PractitionerLocation = null
                    };

                    all.Add(p);
                }

                // -------------------------
                // PAGINACIÓN NEXT
                // -------------------------
                bool hasNext = false;

                if (root["link"] is JsonArray links)
                {
                    foreach (var l in links)
                    {
                        if (l?["relation"]?.ToString() == "next")
                        {
                            hasNext = true;
                            break;
                        }
                    }
                }

                if (!hasNext)
                    break;

                page++;
            }

            return all;
        }

        // ACTUALIZA LA BASE DE DATOS PROPIA CADA 24 HORAS
        // AHORA USA LA FUNCIÓN NUEVA QUE MEZCLA LOS DOS JSON (PRACTITIONER + SPECIALITY)
        public async Task SyncPractitionersToDatabaseAsync()
        {
            // 🔥 Ahora la fuente OFICIAL es GetAllDoctorsWithSpecialityAsync
            var modMedDoctors = await GetAllDoctorsWithSpecialityAsync();

            foreach (var d in modMedDoctors)
            {
                var existing = await _db.Practitioners
                    .FirstOrDefaultAsync(x => x.PractitionerIdentifier == d.Id);

                if (existing == null)
                {
                    var newPractitioner = new Practitioner
                    {
                        PractitionerIdentifier = d.Id,
                        PractitionerName = d.Given ?? "",
                        PractitionerLastName = d.Family ?? "",
                        PractitionerStatus = d.Active,
                        PractitionerSpecialty = d.Specialty ?? "",
                        PractitionerPhotoRef = null,
                        PractitionerProvider = null,
                        PractitionerOnLine = false,
                        PractitionerLocation = null
                    };

                    await _db.Practitioners.AddAsync(newPractitioner);
                }
                else
                {
                    existing.PractitionerName = d.Given ?? existing.PractitionerName;
                    existing.PractitionerLastName = d.Family ?? existing.PractitionerLastName;
                    existing.PractitionerStatus = d.Active;
                    existing.PractitionerSpecialty = d.Specialty ?? existing.PractitionerSpecialty;
                }
            }

            await _db.SaveChangesAsync();
        }


        public async Task<List<DoctorWithSpecialityDto>> GetAllDoctorsWithSpecialityAsync()
        {
            var token = _configuration["ModMedSettings:AccessToken"];

            // 1️⃣ Primer JSON: Practitioners base
            string baseJson = await _modMedRepository.GetAsync("Practitioner", token);
            JsonNode? baseRoot = JsonNode.Parse(baseJson);

            var doctors = new Dictionary<string, DoctorWithSpecialityDto>(StringComparer.OrdinalIgnoreCase);

            if (baseRoot?["entry"] is JsonArray baseEntries)
            {
                foreach (var entry in baseEntries)
                {
                    var resource = entry?["resource"];
                    if (resource == null) continue;

                    // id
                    long id = 0;
                    string? idStr = resource["id"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(idStr))
                        long.TryParse(idStr, out id);

                    // active
                    bool active = resource["active"]?.GetValue<bool>() ?? false;

                    // name → family + given
                    string? family = null;
                    string? given = null;

                    if (resource["name"] is JsonArray nameArray)
                    {
                        var nameObj = nameArray.FirstOrDefault();
                        family = nameObj?["family"]?.ToString();

                        if (nameObj?["given"] is JsonArray givenArray)
                            given = givenArray.FirstOrDefault()?.ToString();
                    }

                    if (string.IsNullOrWhiteSpace(family) || string.IsNullOrWhiteSpace(given))
                        continue;

                    var key = BuildNameKey(family, given);

                    if (!doctors.ContainsKey(key))
                    {
                        doctors[key] = new DoctorWithSpecialityDto
                        {
                            Id = id,
                            Active = active,
                            Family = family,
                            Given = given,
                            Specialty = null
                        };
                    }
                }
            }

            // 2️⃣ Segundo JSON: Practitioners con qualification/display (especialidad)
            string specJson = await _modMedRepository.GetAsync("Practitioner?type=ref", token);
            JsonNode? specRoot = JsonNode.Parse(specJson);

            if (specRoot?["entry"] is JsonArray specEntries)
            {
                foreach (var entry in specEntries)
                {
                    var resource = entry?["resource"];
                    if (resource == null) continue;

                    // name → family + given
                    string? family = null;
                    string? given = null;

                    if (resource["name"] is JsonArray nameArray)
                    {
                        var nameObj = nameArray.FirstOrDefault();
                        family = nameObj?["family"]?.ToString();

                        if (nameObj?["given"] is JsonArray givenArray)
                            given = givenArray.FirstOrDefault()?.ToString();
                    }

                    if (string.IsNullOrWhiteSpace(family) || string.IsNullOrWhiteSpace(given))
                        continue;

                    var key = BuildNameKey(family, given);

                    // Solo actualizamos si ese doctor ya existe del primer JSON
                    if (!doctors.TryGetValue(key, out var dto))
                        continue;

                    // qualification → code.coding[].display
                    if (resource["qualification"] is JsonArray qualArray)
                    {
                        dto.Specialty = MergeSpecialties(dto.Specialty, qualArray);
                    }
                }
            }

            return doctors.Values.ToList();
        }


        // 🔧 Construye la clave "FAMILY|GIVEN" en MAYÚSCULAS
        private static string BuildNameKey(string family, string given)
        {
            return $"{family.Trim().ToUpperInvariant()}|{given.Trim().ToUpperInvariant()}";
        }


        // 🔧 Mezcla especialidades sin duplicar
        private static string MergeSpecialties(string? existingSpecialty, JsonArray qualArray)
        {
            var all = new List<string>();

            // specialties que ya tenía el doctor
            if (!string.IsNullOrWhiteSpace(existingSpecialty))
            {
                all.AddRange(
                    existingSpecialty
                        .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                );
            }

            // specialties nuevas
            foreach (var qual in qualArray)
            {
                var codingArray = qual?["code"]?["coding"] as JsonArray;
                if (codingArray == null) continue;

                foreach (var cod in codingArray)
                {
                    var display = cod?["display"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(display))
                        all.Add(display);
                }
            }

            var distinct = all

                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase);

            return string.Join(", ", distinct);
        }



    }
}
