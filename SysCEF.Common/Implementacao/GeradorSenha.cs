using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SysCEF.Common.Interface;

namespace SysCEF.Common.Implementacao
{
    public class GeradorSenha : IGeradorSenha
    {
        #region Variáveis

        private const int COMPRIMENTO_SENHA = 8;
        private static char[] _CaracteresMaiusculos;
        private static char[] _CaracteresMinusculos;
        private static char[] _CaracteresNumericos;
        private static char[] _CaracteresValidos;
        private StringBuilder _Senha;

        #endregion

        #region Construtor

        static GeradorSenha()
        {
            PopularTodosCaracteresValidos();
            PopularCaracteresMaiusculos();
            PopularCaracteresMinusculos();
            PopulateCaracteresNumericos();
        }

        #endregion

        #region Métodos

        public string Gerar()
        {
            Inicializar();

            PrepararSenhaAleatoria();
            AdicionarCaracterMaiusculo();
            AdicionarCaracterMinusculo();
            AdicionarCaracterNumerico();

            return _Senha.ToString();
        }

        private static void PopularTodosCaracteresValidos()
        {
            _CaracteresValidos = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToArray();
        }

        private static void PopularCaracteresMaiusculos()
        {
            _CaracteresMaiusculos = _CaracteresValidos.Where(p => p >= 65 && p <= 90).ToArray();
        }

        private static void PopularCaracteresMinusculos()
        {
            _CaracteresMinusculos = _CaracteresValidos.Where(p => p >= 97 && p <= 122).ToArray();
        }

        private static void PopulateCaracteresNumericos()
        {
            _CaracteresNumericos = _CaracteresValidos.Where(p => p >= 48 && p <= 57).ToArray();
        }

        private void Inicializar()
        {
            _Senha = new StringBuilder();
        }

        private void PrepararSenhaAleatoria()
        {
            for (var i = 0; i < COMPRIMENTO_SENHA - 3; i++)
                _Senha.Append(ObterCaracterAleatorio(_CaracteresValidos));
        }

        private void AdicionarCaracterMaiusculo()
        {
            InserirCaractereLugarAleatorio(_CaracteresMaiusculos);
        }

        private void AdicionarCaracterMinusculo()
        {
            InserirCaractereLugarAleatorio(_CaracteresMinusculos);
        }

        private void AdicionarCaracterNumerico()
        {
            InserirCaractereLugarAleatorio(_CaracteresNumericos);
        }

        private void InserirCaractereLugarAleatorio(IList<char> caracteresValidos)
        {
            var ch = ObterCaracterAleatorio(caracteresValidos);
            var i = ObterNumeroAleatorio(_Senha.Length);

            _Senha.Insert(i, ch);
        }

        private static int ObterNumeroAleatorio(int max)
        {
            var bytes = new byte[4];
            RandomNumberGenerator.Create().GetBytes(bytes);
            return Math.Abs(BitConverter.ToInt32(bytes, 0))%(max + 1);
        }

        private static char ObterCaracterAleatorio(IList<char> caracteresValidos)
        {
            var i = ObterNumeroAleatorio(caracteresValidos.Count - 1);

            return caracteresValidos[i];
        }

        #endregion
    }
}