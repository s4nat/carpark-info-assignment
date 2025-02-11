using System.IO;
using System.Threading.Tasks;

namespace CarParkInfo.Core.Interfaces;

public interface ICsvImportService
{
    Task ImportCarParksAsync(Stream csvStream);
}