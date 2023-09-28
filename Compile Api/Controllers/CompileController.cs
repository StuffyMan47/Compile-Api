using Microsoft.AspNetCore.Mvc;
using Compile_Api.Models;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Compile_Api.Analyzer;

namespace Compile_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompileController: ControllerBase
    {
        [HttpPost]
        public Response GetCode(User user)
        {
            Response response = new Response();

            string[] trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);

            List<PortableExecutableReference> references = new List<PortableExecutableReference>();

            foreach (var refAsm in trustedAssembliesPaths)
            {
                references.Add(MetadataReference.CreateFromFile(refAsm));
            }

            string json = JsonConvert.SerializeObject(user.userCode);

            Console.WriteLine(json);
            var compiler = new Compiler("First.Program", user.userCode, new[] { typeof(Console) });
            var type = compiler.Compile();
            if (!string.IsNullOrEmpty(compiler.error))
                //Запись ошибок компиляции
                response.compilationErrors = compiler.error;
            else
            {
                Analyzer.Analyzer staticAnalyzer = new Analyzer.Analyzer(user.userCode);
                staticAnalyzer.StaticAnalyzer();
                response.syntaxErrors = staticAnalyzer.Warnings.ToString();
            }

            if (type == null)
            {
                Console.WriteLine(user.userCode);
            }
            else
            {
                TextWriter oldWriter = Console.Out;
                FileStream fs = new FileStream("Test.txt", FileMode.Create);
                // First, save the standard output.
                TextWriter tmp = Console.Out;
                StreamWriter sw = new StreamWriter(fs);
                Console.SetOut(sw);
                type.GetMethod("Main").Invoke(null, null);
                Console.SetOut(oldWriter);
                sw.Close();
                using (StreamReader file = new StreamReader("Test.txt"))
                {
                    //Вывод скомпилированного кода
                    string fileString = file.ReadToEnd();
                    response.compilationResult = fileString;
                }

            }

            return response;
        }
    }
}
