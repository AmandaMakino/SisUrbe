using System.ComponentModel;

namespace SysCEF.Model
{
    #region Geral
    public enum EnumTipoImportacao
    {
        [Description("Fonte")]
        Fonte = 0,
        [Description("Linha")]
        Linha = 1,
        [Description("Produto")]
        Produto = 2
    }

    public enum EnumPerfil
    {
        [Description("Administrador")]
        Administrador = 0,
        [Description("Usuário comum")]
        UsuarioComum = 1,
    }

    public enum EnumStatusLaudo
    {
        [Description("Importado")]
        Importado = 0,
        [Description("Agendado")]
        AFazer = 1,
        [Description("Em Andamento")]
        EmAndamento = 2,
        [Description("Concluído")]
        Concluido = 3
    }

    public enum EnumQuantidade
    {
        [Description("0")]
        Zero = 0,
        [Description("1")]
        Um = 1,
        [Description("2")]
        Dois = 2,
        [Description("3")]
        Tres = 3,
        [Description("4")]
        Quatro = 4,
        [Description("5")]
        Cinco = 5,
        [Description("6")]
        Seis = 6,
        [Description("7")]
        Sete = 7,
        [Description("8")]
        Oito = 8,
        [Description("9")]
        Nove = 9,
        [Description("10")]
        Dez = 10
    }

    public enum EnumSimOuNao
    {
        [Description("Sim")]
        Sim = 0,
        [Description("Não")]
        Nao = 1
    }

    public enum EnumDisposicaoRadioButtons
    {
        Horizontal = 0,
        Vertical = 1
    }
    #endregion

    #region Identificação
    public enum EnumAtividade
    {
        [Description("301 - AV IM URBANO - LAUDO MOD SIMPLIFICADO")]
        LaudoModSimplificado = 301
    }

    public enum EnumProduto
    {
        [Description("CARTA DE CRÉDITO INDIVIDUAL")]
        CartaCreditoIndividual = 5,
        [Description("IMÓVEL COMERCIAL - PESSOA JURÍDICA")]
        ImovelComercialPessoaJuridica = 122
    }

    public enum EnumLinha
    {
        [Description("VALOR DE MERCADO")]
        ValorMercado = 42
    }
    
    public enum EnumFonte
    {
        [Description("FGTS")]
        FGTS = 9,
        [Description("SBPE")]
        SBPE = 29
    }
    #endregion

    #region Caracterização da Região
    public enum EnumUsosPredominantes
    {
        [Description("Residencial Unifamiliar")]
        ResUni = 0,
        [Description("Residencial Multifamiliar")]
        ResMult = 1,
        [Description("Comercial")]
        Com = 2,
        [Description("Industrial")]
        Ind = 3,
    }

    public enum EnumInfraEstruturaUrbana
    {
        [Description("Água")]
        Ag = 0,
        [Description("Esgoto Sanitário")]
        Esg = 1,
        [Description("Energia Elétrica")]
        EE = 2,
        [Description("Telefone")]
        Tel = 3,
        [Description("Pavimentação")]
        Pav = 4,
        [Description("Esgoto Pluvial")]
        Plu = 5,
        [Description("Gás Canalizado")]
        Gas = 6,
        [Description("Iluminação Pública")]
        Il = 7
    }

    public enum EnumServicoPublicoComunitario
    {
        [Description("Coleta de Lixo")]
        Lixo = 0,
        [Description("Transporte Coletivo")]
        Tran = 1,
        [Description("Comércio")]
        Come = 2,
        [Description("Rede Bancária")]
        ReBa = 3,
        [Description("Escola")]
        Esco = 4,
        [Description("Saúde")]
        Saud = 5,
        [Description("Segurança")]
        Segu = 6,
        [Description("Lazer")]
        Laze = 7
    }
    #endregion

    #region Terreno
    public enum EnumFormaTerreno
    {
        [Description("Irregular")]
        Irregular = 0,
        [Description("Quadrada")]
        Quadrada = 1,
        [Description("Retangular")]
        Retangular = 2
    }

    public enum EnumCotaGreide
    {
        [Description("Abaixo")]
        Abaixo = 0,
        [Description("Acima")]
        Acima = 1
    }

    public enum EnumInclinacaoTerreno
    {
        [Description("Acidentado")]
        Acidentado = 0,
        [Description("Aclive/Declive > 10%")]
        AcliveDeclive = 1,
        [Description("Plano")]
        Plano = 2,
        [Description("Semi-plano")]
        SemiPlano = 3
    }

    public enum EnumSituacaoTerreno
    {
        [Description("Esquina")]
        Esquina = 0,
        [Description("Meio de quadra")]
        MeioQuadra = 1,
        [Description("Quadra inteira")]
        QuadraInteira = 2,
        [Description("Outros")]
        Outros = 3
    }

    public enum EnumSuperficieTerreno
    {
        [Description("Alagável")]
        Alagavel = 0,
        [Description("Brejoso")]
        Brejoso = 1,
        [Description("Seco")]
        Seco = 2,
        [Description("Outros")]
        Outros = 3
    }
    #endregion

    #region Edificação
    public enum EnumTipoEdificacao
    {
        [Description("Apartamento")]
        Apartamento = 0,
        [Description("Loja")]
        Loja = 1,
        [Description("Sala")]
        Sala = 2,
        [Description("Outros")]
        Outros = 3
    }

    public enum EnumPosicaoEdificacao
    {
        [Description("Frente/Canto")]
        FrenteCanto = 0,
        [Description("Frente/Meio")]
        FrenteMeio = 1,
        [Description("Fundos/Canto")]
        FundosCanto = 2,
        [Description("Fundos/Meio")]
        FundosMeio = 3,
        [Description("Lateral")]
        Lateral = 4,
        [Description("Outros")]
        Outros = 5
    }

    public enum EnumPadraoAcabamento
    {
        [Description("Alto")]
        Alto = 0,
        [Description("Normal/Alto")]
        NormalAlto = 1,
        [Description("Normal")]
        Normal = 2,
        [Description("Normal/Baixo")]
        NormalBaixo = 3,
        [Description("Baixo")]
        Baixo = 4,
        [Description("Baixo/Mínimo")]
        BaixoMinimo = 5,
        [Description("Mínimo")]
        Minimo = 6
    }

    public enum EnumEstadoConservacao
    {
        [Description("Bom")]
        Bom = 0,
        [Description("Regular")]
        Regular = 1,
        [Description("Ruim (residual)")]
        Ruim = 2
    }

    public enum EnumTetos
    {
        [Description("Forro")]
        Forro = 0,
        [Description("Lajes")]
        Lajes = 1,
        [Description("Telhado aparente")]
        Telhado = 2,
        [Description("Outros")]
        Outros = 3
    }

    public enum EnumFechamentoParedes
    {
        [Description("Alvenaria")]
        Alvenaria = 0,
        [Description("Alvenaria/Madeira")]
        MadeiraAlvenaria = 1,
        [Description("Madeira")]
        Madeira = 2,
        [Description("Outros")]
        Outros = 3
    }

    public enum EnumUsosPredio
    {
        [Description("Comercial")]
        Comercial = 0,
        [Description("Industrial")]
        Industrial = 1,
        [Description("Misto")]
        Misto = 2,
        [Description("Residencial")]
        Residencial = 3,
        [Description("Outros")]
        Outros = 4
    }

    public enum EnumQuantidadeMaior
    {
        [Description("0")]
        Zero = 0,
        [Description("1")]
        Um = 1,
        [Description("2")]
        Dois = 2,
        [Description("3")]
        Tres = 3,
        [Description("4")]
        Quatro = 4,
        [Description("5")]
        Cinco = 5,
        [Description("6")]
        Seis = 6,
        [Description("7")]
        Sete = 7,
        [Description("8")]
        Oito = 8,
        [Description("9")]
        Nove = 9,
        [Description("10")]
        Dez = 10,
        [Description("11")]
        Onze = 11,
        [Description("12")]
        Doze = 12,
        [Description("13")]
        Treze = 13,
        [Description("14")]
        Quatorze = 14,
        [Description("15")]
        Quize = 15,
        [Description("16")]
        Dezesseis = 16,
        [Description("17")]
        Dezessete = 17,
        [Description("18")]
        Dezoito = 18,
        [Description("19")]
        Dezenove = 19,
        [Description("20")]
        Vinte = 20,
        [Description("21")]
        VinteUm = 21,
        [Description("22")]
        VinteDois = 22,
        [Description("23")]
        VinteTres = 23,
        [Description("24")]
        VinteQuatro = 24,
        [Description("25")]
        VinteCinco = 25,
        [Description("26")]
        VinteSeis = 26,
        [Description("27")]
        VinteSete = 27,
        [Description("28")]
        VinteOito = 28,
        [Description("29")]
        VinteNove = 29,
        [Description("30")]
        Trinta = 30
    }

    public enum EnumPosicaoPredio
    {
        [Description("Isolado/Frente do terreno")]
        IsoladoFrenteTerreno = 0,
        [Description("Isolado/Centro do terreno")]
        IsoladoCentroTerreno = 1,
        [Description("Junto a uma das laterais")]
        JuntoLateral = 2,
        [Description("Junto aos fundos")]
        JuntoFundos = 3,
        [Description("Geminada em uma das laterais")]
        GeminadaEmUmaLateral = 4,
        [Description("Geminada nas laterais")]
        GeminadaLaterais = 5,
        [Description("Ocupa todo o terreno")]
        OcupaTodoTerreno = 6
    }
    #endregion

    #region Avaliação
    public enum EnumMetodoDefinicaoValor
    {
        [Description("Global")]
        Global = 0,
        [Description("Itemizada")]
        Itemizada = 1
    }
    
    public enum EnumMetodologiaAvaliacao
    {
        [Description("Comparativo de dados do mercado")]
        ComparativoDadosMercado = 0,
        [Description("Comparativo de custo de reprodução de benfeitorias")]
        ComparativoCustoReproducaoBenfeitorias = 1,
        [Description("Da renda")]
        Renda = 2,
        [Description("Involutivo")]
        Involutivo = 3,
        [Description("Residual")]
        Residual = 4
    }

    public enum EnumDesempenhoMercado
    {
        [Description("Aquecido")]
        Aquecido = 0,
        [Description("Normal")]
        Normal = 1,
        [Description("Recessivo")]
        Recessivo = 2
    }
    
    public enum EnumAbsorcaoMercado
    {
        [Description("Rápida")]
        Rapida = 0,
        [Description("Demorada")]
        Demorada = 1,
        [Description("Muito difícil")]
        MuitoDificil = 2
    }

    public enum EnumNivelImobiliario
    {
        [Description("Alto")]
        Alto = 0,
        [Description("Médio")]
        Medio = 1,
        [Description("Baixo")]
        Baixo = 2
    }
    #endregion

    #region Informações Complementares
    public enum EnumFatoresLiquidezValorImovel
    {
        [Description("Valorizantes")]
        Valorizantes = 0,
        [Description("Desvalorizantes")]
        Desvalorizantes = 1,
        [Description("Nenhum")]
        Nenhum = 2
    }
    #endregion
}
