using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Core;
using DataAccess;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using SysCEF.Web.Models;

namespace SysCEF.Web.Helpers
{
    public class OpcoesHelper
    {
        #region Propriedades
        public Laudo Laudo { get; set; }
        #endregion

        #region Construtor
        public OpcoesHelper()
        {
        }

        public OpcoesHelper(Laudo laudo)
        {
            Laudo = laudo;
        }
        #endregion

        #region Métodos
        public List<SelectListItem> ObterOpcoesEstado(IEnumerable<Estado> estados)
        {
            return (from e in estados
                    select new SelectListItem
                    {
                        Selected = Laudo != null && e.EstadoID == Laudo.Imovel.Cidade.Estado.EstadoID,
                        Text = e.Nome.ToUpper(CultureInfo.InvariantCulture),
                        Value = e.EstadoID.ToString(CultureInfo.InvariantCulture)
                    }).ToList();
        }

        public List<SelectListItem> ObterOpcoesCidade(IEnumerable<Cidade> cidades)
        {
            var listaCidades = (from c in cidades
                                select new SelectListItem
                                           {
                                               Selected = Laudo != null && c.CidadeID == Laudo.Imovel.Cidade.CidadeID,
                                               Text = c.Nome.ToUpper(CultureInfo.InvariantCulture),
                                               Value = c.CidadeID.ToString(CultureInfo.InvariantCulture)
                                           }).ToList();

            return listaCidades;
        }

        public List<SelectListItem> ObterOpcoesTipoLogradouro(IEnumerable<TipoLogradouro> tiposLogradouro)
        {
            return (from tl in tiposLogradouro
                    select new SelectListItem
                    {
                        Selected = Laudo != null && tl.TipoLogradouroID == Laudo.Imovel.TipoLogradouro.TipoLogradouroID,
                        Text = tl.Descricao.ToUpper(CultureInfo.InvariantCulture),
                        Value = tl.TipoLogradouroID.ToString(CultureInfo.InvariantCulture)
                    }).ToList();
        }

        public List<SelectListItem> ObterOpcoesResponsaveisTecnicos(IEnumerable<Usuario> usuarios)
        {
            var lista = (from u in usuarios
                         orderby u.Nome
                         select new SelectListItem
                                    {
                                        Selected = Laudo != null && Laudo.ResponsavelTecnico != null && u.UsuarioId == Laudo.ResponsavelTecnico.UsuarioId,
                                        Text = u.Nome,
                                        Value = u.UsuarioId.ToString(CultureInfo.InvariantCulture)
                                    }).ToList();

            lista.Insert(0, new SelectListItem
                                {
                                    Text = "<Selecione>",
                                    Value = "0",
                                    Selected = !lista.Any(l => l.Selected)
                                });

            return lista;
        }

        public List<SelectListItem> ObterOpcoesRepresentantesLegais(IEnumerable<Usuario> usuarios)
        {
            var lista = (from u in usuarios
                         orderby u.Nome
                         select new SelectListItem
                         {
                             Selected = Laudo != null && Laudo.RepresentanteLegalEmpresa != null && u.UsuarioId == Laudo.RepresentanteLegalEmpresa.UsuarioId,
                             Text = u.Nome,
                             Value = u.UsuarioId.ToString(CultureInfo.InvariantCulture)
                         }).ToList();

            lista.Insert(0, new SelectListItem
            {
                Text = "<Selecione>",
                Value = "0",
                Selected = !lista.Any(l => l.Selected)
            });

            return lista;
        }
        
        public List<SelectListItem> ObterOpcoesEnum<T>() where T : struct
        {
            var enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("This method is only valid for Enumerations");
            }

            var listaOpcoes = new List<SelectListItem>();

            foreach (var opcao in enumType.GetFields())
            {
                var attributes = (DescriptionAttribute[])opcao.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length <= 0)
                    continue;
                
                var value = (T)opcao.GetRawConstantValue();

                listaOpcoes.Add(new SelectListItem
                                    {
                                        Text = attributes[0].Description,
                                        Value = value.ToString()
                                    });
            }

            return listaOpcoes;
        }
        #endregion
    }
}