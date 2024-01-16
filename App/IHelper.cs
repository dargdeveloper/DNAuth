namespace DotNet.Docker
{
    public interface IHelper
    {
        public Task<string> CreateFunction(FunctionModel functionModel);
        public Task<string> DeleteFunction(string functionId);
    }
}
