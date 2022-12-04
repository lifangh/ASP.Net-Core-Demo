using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Common.Helper
{
    /// <summary>
    ///  MD5 加密
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// 16 MD5 加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string password)
        {
            var md5 = new MD5CryptoServiceProvider();

            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password), 4, 8));

            t2 = t2.Replace("-", string.Empty);
            return t2;
        }

        /// <summary>
        /// 32 MD5 加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string MD5Encrypt32(string password = "")
        {
            string pwd = string.Empty;

            try
            {
                if(!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
                {
                    MD5 md5 = MD5.Create();
                    // 加密后是一个字节类型的数组，这里要注意编码unf-8/Unicode 的选择
                    byte[] bytes = md5.ComputeHash(Encoding.Default.GetBytes(password));

                    //通过循环，将字节类型的数组转成字符串，此字符串是常规字符格式化后所得
                    foreach (var item in bytes)
                    {
                        //将得到的字符串使用十六进制类型格式，格式后的字符串是小写字母，如果使用大写（X）,则格式后的字符是大写字符
                        pwd = string.Concat(pwd,item.ToString("X2"));
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception($"错误的 password 字符串：【{password}】");
            }
            return pwd;
        }
        /// <summary>
        /// 64 MD5 加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string MD5EncryptBase64(string password)
        {
            MD5 md5 = MD5.Create();
            // 加密后是一个字节类型的数组，这里要注意编码unf-8/Unicode 的选择
            byte[] bytes = md5.ComputeHash(Encoding.Default.GetBytes(password));//16字节数组

            return Convert.ToBase64String(bytes);//生成的base64字符串长度是24
        }
    }
}
