using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface ITickService
{
    /// <summary>
    /// Asynchronously retrieves all climbing ticks from the database and converts them to DTOs.
    /// Returns a service result indicating success, not found, or an error in case of an exception.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="ServiceResult{T}"/> 
    /// with a collection of <see cref="TickDto"/>. Returns a "Not Found" result if no ticks exist.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Caught and returned as an error result if a null value is encountered while mapping ticks.
    /// </exception>
    /// <exception cref="Exception">
    /// Caught and returned as an error result if an unexpected error occurs.
    /// </exception>
    Task<ServiceResult<IEnumerable<TickDto>>> GetAllAsync();
    
    /// <summary>
    /// Asynchronously retrieves a climbing tick by its ID and converts it to a DTO.
    /// Returns a service result indicating success, not found, or an error in case of an exception.
    /// </summary>
    /// <param name="id">The unique identifier of the tick to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="ServiceResult{T}"/> 
    /// with the corresponding <see cref="TickDto"/>. Returns a "Not Found" result if no tick is found.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Caught and returned as an error result if an invalid argument is encountered.
    /// </exception>
    /// <exception cref="Exception">
    /// Caught and returned as an error result if an unexpected error occurs.
    /// </exception>
    Task<ServiceResult<TickDto>> GetByIdAsync(int id);
    
    /// <summary>
    /// Asynchronously retrieves multiple climbing ticks by a list of IDs and converts them to DTOs.
    /// Returns a service result indicating success, not found, or an error in case of an exception.
    /// </summary>
    /// <param name="tickIds">A list of tick IDs to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="ServiceResult{T}"/> 
    /// with a list of <see cref="TickDto"/>. Returns a "Not Found" result if no ticks are found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Caught and returned as an error result if a null value is encountered.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Caught and returned as an error result if the provided list of IDs is empty.
    /// </exception>
    /// <exception cref="Exception">
    /// Caught and returned as an error result if an unexpected error occurs.
    /// </exception>
    Task<ServiceResult<List<TickDto>>> GetByListOfIdsAsync(List<int> tickIds);
    
    /// <summary>
    /// Asynchronously adds a new climbing tick to the database and returns the saved tick as a DTO.
    /// Returns a service result indicating success or an error in case of an exception.
    /// </summary>
    /// <param name="tickDto">The tick data transfer object to be added.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="ServiceResult{T}"/> 
    /// with the saved <see cref="TickDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Caught and returned as an error result if the provided tick DTO is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Caught and returned as an error result if an issue occurs while saving the tick.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Caught and returned as an error result if a database update error occurs.
    /// </exception>
    /// <exception cref="Exception">
    /// Caught and returned as an error result if an unexpected error occurs.
    /// </exception>
    Task<ServiceResult<TickDto>> AddAsync(TickDto tickDto);
    
    /// <summary>
    /// Asynchronously updates an existing climbing tick in the database.
    /// Returns a service result indicating success or an error in case of an exception.
    /// </summary>
    /// <param name="tickDto">The tick data transfer object containing updated information.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="ServiceResult{T}"/> 
    /// with the updated <see cref="TickDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Caught and returned as an error result if the provided tick DTO is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Caught and returned as an error result if an issue occurs while updating the tick.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Caught and returned as an error result if a database update error occurs.
    /// </exception>
    /// <exception cref="Exception">
    /// Caught and returned as an error result if an unexpected error occurs.
    /// </exception>
    Task<ServiceResult<TickDto>> UpdateAsync(TickDto tickDto);
    
    /// <summary>
    /// Asynchronously deletes a climbing tick from the database by its ID.
    /// Returns a service result indicating success or an error in case of an exception.
    /// </summary>
    /// <param name="id">The unique identifier of the tick to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="ServiceResult{T}"/> 
    /// with a boolean indicating whether the deletion was successful.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Caught and returned as an error result if the provided ID is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Caught and returned as an error result if an issue occurs while deleting the tick.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Caught and returned as an error result if a database update error occurs.
    /// </exception>
    /// <exception cref="Exception">
    /// Caught and returned as an error result if an unexpected error occurs.
    /// </exception>
    Task<ServiceResult<bool>> DeleteAsync(int id);
    
    /// <summary>
    /// Asynchronously saves a climbing tick to the database using a TickDTO.
    /// This method retrieves or creates the associated climb entity, converts the DTO to an entity,
    /// and persists it via the repository. Returns the saved tick as a DTO.
    /// Throws an exception if the operation does not result in exactly one record being added.
    /// </summary>
    /// <param name="tickDto">The tick data transfer object containing tick details.</param>
    /// <returns>A task representing the asynchronous operation, containing the saved TickDTO.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="tickDto"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the number of records saved is not exactly one.</exception>
    Task<TickDto> SaveTickAsync(TickDto tickDto);
}