using System.Text.Json;
using AutoMapper;
using MySqlConnector;
using riskwatch.api.search.Database;
using riskwatch.api.search.Features.Common.Dto;
using Dapper;
using Nest;
using riskwatch.api.search.Features.FuzzySearch.Dto;
using Aliases = riskwatch.api.search.Entities.Aliases;

namespace riskwatch.api.search.Features.FuzzySearch.Service.EntityRecordService;

public class EntityRecordService : IEntityRecordService
{
    private readonly IMapper _mapper;
    public EntityRecordService(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task<List<EntityRecordDto>> GetAllAsync()
    {
        using var connection = new MySqlConnection(GlobalDbConnection.MysqlConnection);
        
        var query = @"SELECT Entity.Id, Entity.Name, 
                        CASE 
                          WHEN Individual.EntityId IS NOT NULL THEN 'Individual' 
                          WHEN Corporate.EntityId IS NOT NULL THEN 'Corporate' 
                          ELSE 'Unknown' 
                        END AS Type, 
                        Individual.BirthDate,
                        COALESCE(Individual.Aliases, Corporate.Aliases) AS Aliases, 
                        COALESCE(Individual.AddressDetails, Corporate.AddressDetails) AS AddressDetails, 
                        COALESCE(Individual.ContactDetails, Corporate.ContactDetails) AS ContactDetails, 
                        COALESCE(Individual.RiskwatchNumber, Corporate.RiskwatchNumber) AS RiskwatchNumber 
                        FROM Entity 
                        LEFT JOIN Individual ON Entity.Id = Individual.EntityId 
                        LEFT JOIN Corporate ON Entity.Id = Corporate.EntityId;";
        
        var rawResult  = await connection.QueryAsync<RawEntityRecordDto>(query);
        var resultList = new List<EntityRecordDto>();
        
        foreach (var rawDto in rawResult)
        {
            var entityDto = new EntityRecordDto
            {
                Name = rawDto.Name,
                Type = rawDto.Type,
                BirthDate = rawDto.BirthDate,
                RiskwatchNumber = rawDto.RiskwatchNumber,
                Aliases = !string.IsNullOrEmpty(rawDto.Aliases) ? JsonSerializer.Deserialize<List<Aliases>>(rawDto.Aliases) : new List<Aliases>(),
                //Add other properties here if needed in elastic search
            };
            resultList.Add(entityDto);
        }
        Console.WriteLine("Count:"+resultList.Count);
        return resultList;
    }

    public async Task<List<SearchResultDto>> GetAndMapEntityRecordsAsync()
    {
        var result = await GetAllAsync();
        var records = _mapper.Map<List<SearchResultDto>>(result);
        // Update the Suggest field in each DTO
        foreach (var record in records)
        {
            record.Suggest = new CompletionField
            {
                Input = new List<string> { record.Name }
                    .Where(input => !string.IsNullOrEmpty(input)) // Filter out null or empty values
                    .ToList()
            };
        }
        return records;
    }
}