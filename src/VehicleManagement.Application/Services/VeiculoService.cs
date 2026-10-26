using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Exceptions;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Mappings;
using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Interfaces;

namespace VehicleManagement.Application.Services;

public class VeiculoService : IVeiculoService
{
    private readonly IVeiculoRepository _veiculoRepository;

    public VeiculoService(IVeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }

    public async Task<VeiculoDto> AdicionarAsync(AdicionarVeiculoInput input, CancellationToken cancellationToken)
    {
        var veiculo = new Veiculo(
            input.Descricao,
            input.Marca,
            input.Modelo,
            input.Opcionais,
            input.Valor);

        await _veiculoRepository.AdicionarAsync(veiculo, cancellationToken);

        return VeiculoMapper.ToDto(veiculo);
    }

    public async Task<VeiculoDto> AtualizarAsync(
        Guid id,
        AtualizarVeiculoInput input,
        CancellationToken cancellationToken)
    {
        var veiculo = await ObterEntidadeOuFalharAsync(id, cancellationToken);

        veiculo.Atualizar(
            input.Descricao,
            input.Marca,
            input.Modelo,
            input.Opcionais,
            input.Valor);

        await _veiculoRepository.AtualizarAsync(veiculo, cancellationToken);

        return VeiculoMapper.ToDto(veiculo);
    }

    public async Task ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        var veiculo = await ObterEntidadeOuFalharAsync(id, cancellationToken);
        await _veiculoRepository.ExcluirAsync(veiculo, cancellationToken);
    }

    public async Task<VeiculoDto> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var veiculo = await ObterEntidadeOuFalharAsync(id, cancellationToken);
        return VeiculoMapper.ToDto(veiculo);
    }

    public async Task<IReadOnlyCollection<VeiculoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var veiculos = await _veiculoRepository.ListarAsync(cancellationToken);

        return veiculos
            .Select(VeiculoMapper.ToDto)
            .ToList();
    }

    private async Task<Veiculo> ObterEntidadeOuFalharAsync(Guid id, CancellationToken cancellationToken)
    {
        var veiculo = await _veiculoRepository.ObterPorIdAsync(id, cancellationToken);

        if (veiculo is null)
        {
            throw new NotFoundException($"Veículo com Id '{id}' não foi encontrado.");
        }

        return veiculo;
    }
}
