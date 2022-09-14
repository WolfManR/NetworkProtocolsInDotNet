namespace ClinicService.Data;

public interface IEntity<TId>
{
    public TId Id { get; set; }
}