using System;
using System.Linq;
using WpfFront.Model;

namespace WpfFront.Utilidades
{
    public class Autenticacion
    {

        wmsEntities db = new wmsEntities();

        private String GetEncrypt(string data, string criptKey)
        {
            Crypto cpt = new Crypto(Crypto.SymmProvEnum.DES);
            return cpt.Encrypting(data, criptKey);
        }

        public Usuario ValidarUsuario(string user, string pass)
        {
            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass))
                throw new Exception("Username or password not contains data.");

            try
            {
                string contraseñaEncriptada = GetEncrypt(pass, user);
                Usuario obj = db.Usuario.FirstOrDefault(f => f.NombreUsuario == user && f.Contraseña == contraseñaEncriptada);//Factory.DaoUsuario().Select(user);

                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    throw new Exception("Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception )
            {
                throw new Exception("Error validating user " + user + ".");
            }
        }
    }
}
