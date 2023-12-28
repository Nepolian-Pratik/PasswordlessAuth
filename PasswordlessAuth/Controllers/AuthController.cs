using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordlessAuth.Helpers;
using PasswordlessAuth.Models;
using PasswordlessAuth.Repository;
using System.Security.Cryptography.Xml;
using System.Text;

namespace PasswordlessAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserRepository userRepo { get; set; }

        private RSAEncryption encryption { get; set; }

        public AuthController(IUserRepository userRepository, RSAEncryption encryption)
        {
            userRepo = userRepository;
            this.encryption = encryption;
        }

        //[HttpPost]
        //[Route("SignUp")]
        //public IActionResult Signup(User user)
        //{
        //    try
        //    {
        //        //try to insert the user
        //        dbContext.Users.Add(user);
        //        dbContext.SaveChanges();
        //        return Ok("User created successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            try
            {
                var userFromDb = await userRepo.GetUserByUserName(user.UserName);//dbContext.Users.FirstOrDefault(u => u.Username == user.UserName);
                if (userFromDb == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Invalid username");
                }
                string challenge = GenerateRandomMathProblem();
                string answer = Evaluate(challenge).ToString();
                userFromDb.LastAnswer = answer;

                //string encryptedChallenge = encryption.EncryptWithPublicKey("Hello", "");

                //dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, "Please solve the following problem:");// + encryptedChallenge);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Exception occurred in the server!" + ex.Message);
            }
        }

        public static double Evaluate(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string)row["expression"]);
        }

        public static string GenerateRandomMathProblem()
        {
            Random random = new Random();
            int numberOfOperands = random.Next(5, 11);
            StringBuilder equation = new StringBuilder();

            for (int i = 0; i < numberOfOperands; i++)
            {
                int operand = random.Next(1, 11);
                int operation = random.Next(1, 5);

                // Ensure division by zero does not occur
                if (operation == 4 && operand == 0)
                {
                    operand = random.Next(1, 11);
                }

                equation.Append(operand);
                if (i < numberOfOperands - 1)
                {
                    equation.Append(" ");
                    equation.Append(GetOperator(operation));
                    equation.Append(" ");
                }
            }

            return equation.ToString();
        }

        public static string GetOperator(int operation)
        {
            switch (operation)
            {
                case 1:
                    return "+";

                case 2:
                    return "-";

                case 3:
                    return "*";

                case 4:
                    return "/";

                default:
                    return "";
            }
        }
    }
}