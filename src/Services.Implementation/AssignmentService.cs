using AutoMapper;
using Contexts;
using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;

namespace Services.Implementation
{
    public class AssignmentService(HsmsDbContext context, IMapper mapper) : IAssignmentService
    {
        private readonly HsmsDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<AssignmentDTO>> CreateAssignmentsAsync(IEnumerable<AssignmentForUpdateDTO> assignments)
        {
            var assignmentsDTO = new List<AssignmentDTO>();
            foreach (var assignment in assignments)
            {
                var assignmentDb = _mapper.Map<AssignmentForCreationDTO>(assignment);
                assignmentsDTO.Add(_mapper.Map<AssignmentDTO>(assignmentDb));
                _context.Add(assignmentDb);
            }
            await _context.SaveChangesAsync();
            return assignmentsDTO;
        }

        public async Task DeleteAssignmentsAsync(IEnumerable<Guid> ids)
        {
            var assignmentsDb = new List<Assignment>();
            foreach (var id in ids)
            {
                var assignmentDb = await _context.Assignments.FirstOrDefaultAsync(x => x.Id == id) ??
                    throw new Exception("Không tìm thấy phân công " + id);

                assignmentsDb.Add(assignmentDb);
            }

            _context.RemoveRange(assignmentsDb);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<AssignmentDTO>, MetaData)> GetAssignmentsAsync(AssignmentParameters parameters)
        {
            var assignments = _context.Assignments
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .AsQueryable();

            var count = assignments.Count();

            assignments = parameters.StartYear == null ? assignments :
                assignments.Where(a => a.StartYear == parameters.StartYear);

            assignments = parameters.EndYear == null ? assignments :
                assignments.Where(a => a.EndYear == parameters.EndYear);

            assignments = parameters.Semester == null ? assignments :
                assignments.Where(a => a.Semester == parameters.Semester);

            assignments = parameters.SearchTerm == null ||
                parameters.SearchTerm == string.Empty ? assignments :
                assignments
                .Where(a =>
                a.Teacher.FirstName.Contains(parameters.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                a.Teacher.MiddleName.Contains(parameters.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                a.Teacher.LastName.Contains(parameters.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                a.Teacher.ShortName.Contains(parameters.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                a.Class.Name.Contains(parameters.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                a.Subject.Name.Contains(parameters.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                a.Subject.ShortName.Contains(parameters.SearchTerm, StringComparison.OrdinalIgnoreCase));

            var p = parameters.OrderBy == null ? null : typeof(Assignment).GetProperty(parameters.OrderBy);

            assignments = p == null ? assignments :
                assignments.OrderBy(a => p.GetValue(assignments, null));

            assignments = assignments
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);
            var result = new List<AssignmentDTO>();
            foreach (var assignment in await assignments.ToListAsync())
                result.Add(_mapper.Map<AssignmentDTO>(assignment));

            var metaData = new MetaData
            {
                totalCount = count,
                pageSize = parameters.PageSize,
                currentPage = parameters.PageNumber,
                totalPages = (int)Math.Ceiling(count / (double)parameters.PageSize)
            };
            return (result, metaData);
        }

        public async Task<IEnumerable<AssignmentDTO>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            var assignments = await _context.Assignments.Where(a => ids.Contains(a.Id) && a.IsDeleted == false).ToListAsync();
            var result = new List<AssignmentDTO>();
            foreach (var assignment in assignments)
                result.Add(_mapper.Map<AssignmentDTO>(assignment));
            return result;
        }

        public async Task<IEnumerable<AssignmentDTO>> UpdateAssignmentsAsync(IEnumerable<AssignmentForUpdateDTO> assignments)
        {
            var assignmentsDb = new List<Assignment>();
            foreach (var assignment in assignments)
            {
                var assignmentDb = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == assignment.Id)
                    ?? throw new Exception("Không tìm thấy phân công " + assignment.Id);
                _mapper.Map(assignmentDb, assignment);
                assignmentsDb.Add(assignmentDb);
            }
            await _context.SaveChangesAsync();

            var result = new List<AssignmentDTO>();
            foreach (var assignment in assignmentsDb)
                result.Add(_mapper.Map<AssignmentDTO>(assignment));

            return result;
        }
    }
}
