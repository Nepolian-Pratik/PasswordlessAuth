using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using PwdLessAuth.Models;
using PwdLessAuth.Repository;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace PwdLessAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(UserSignUpDto user)
        {
            try
            {
                var newUser = _userService.CreateUser((User)user);

                return Ok("Created");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            try
            {
                var userFromDb = await _userService.GetUserByUserName(user.UserName);
                if (userFromDb == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Invalid username");
                }
                string challenge = GenerateRandomMathProblem();
                string answer = Evaluate(challenge).ToString();
                userFromDb.LastAnswer = answer;

                string encryptedChallenge = EncryptWithPublicKey("Hello", userFromDb.PublicKey);

                return Ok(new { encryptedChallenge });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpPost]
        //[Route("Validate")]
        //public async Task<IActionResult> Validate(UserLoginDto user)
        //{
        //    try
        //    {
        //        var userFromDb = await _userService.GetUserByUserName(user.UserName);
        //        if (userFromDb == null)
        //        {
        //            return StatusCode(StatusCodes.Status401Unauthorized, "Invalid username");
        //        }
        //        if (userFromDb.LastAnswer != user.Answer)
        //        {
        //            return StatusCode(StatusCodes.Status401Unauthorized, "Invalid answer");
        //        }
        //        return Ok("Validated");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

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

        public static double Evaluate(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string)row["expression"]);
        }

        public static string EncryptWithPublicKey(string plainText, string publicKeyString)
        {
            byte[] pbKeyStringByte = Convert.FromBase64String(publicKeyString);
            publicKeyString = Encoding.UTF8.GetString(pbKeyStringByte);

            // Read the public key using a PemReader
            PemReader pr = new PemReader(new StringReader(publicKeyString));
            AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)pr.ReadObject();

            // Convert the public key to RSA parameters
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();

            csp.ImportParameters(rsaParams);
            var pbKeyStringBytes = Encoding.Unicode.GetBytes(plainText);
            var cipher = csp.Encrypt(pbKeyStringBytes, false);
            return Convert.ToBase64String(cipher);
        }
    }
}