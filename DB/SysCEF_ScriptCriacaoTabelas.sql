
-- Tabelas:

create table [Imovel]
(
	[ImovelID] int identity not null,
	[CidadeID] int not null,
	[TipoLogradouroID] int not null,
	[NomeCliente] nvarchar(150) not null CONSTRAINT [DF_Imovel_NomeCliente] DEFAULT '',
	[Endereco] nvarchar(150) not null CONSTRAINT [DF_Imovel_Endereco] DEFAULT '',
	[Numero] int not null,
	[Complemento] nvarchar(50) not null CONSTRAINT [DF_Imovel_Complemento] DEFAULT '',
	[Bairro] nvarchar(100) not null CONSTRAINT [DF_Imovel_Bairro] DEFAULT '',
	[CEP] nvarchar(10) not null CONSTRAINT [DF_Imovel_CEP] DEFAULT '',
	[IdentificacaoCliente] nvarchar(50) not null CONSTRAINT [DF_Imovel_IdentificacaoCliente] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_Imovel___Version DEFAULT 0
)

alter table [Imovel]
	add constraint [Imovel_PK] primary key ([ImovelID])

create table [ServicoPublicoComunitario]
(
	[ServicoPublicoComunitarioID] int identity not null,
	[LaudoID] int not null,
	[TipoServicoPublicoComunitario] int not null,
	[Descricao] nvarchar(50) not null CONSTRAINT [DF_ServicoPublicoComunitario_Descricao] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_ServicoPublicoComunitario___Version DEFAULT 0
)

alter table [ServicoPublicoComunitario]
	add constraint [ServicoPublicoComunitario_PK] primary key ([ServicoPublicoComunitarioID])

create table [InfraEstruturaUrbana]
(
	[InfraEstruturaUrbanaID] int identity not null,
	[LaudoID] int not null,
	[TipoInfraEstruturaUrbana] int not null,
	[Descricao] nvarchar(50) not null CONSTRAINT [DF_InfraEstruturaUrbana_Descricao] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_InfraEstruturaUrbana___Version DEFAULT 0
)

alter table [InfraEstruturaUrbana]
	add constraint [InfraEstruturaUrbana_PK] primary key ([InfraEstruturaUrbanaID])

create table [Usuario]
(
	[UsuarioId] int identity not null,
	[Nome] nvarchar(100) not null CONSTRAINT [DF_Usuario_Nome] DEFAULT '',
	[Perfil] int not null,
	[Email] nvarchar(50) not null CONSTRAINT [DF_Usuario_Email] DEFAULT '',
	[Senha] nvarchar(255) not null CONSTRAINT [DF_Usuario_Senha] DEFAULT '',
	[DeveDefinirNovaSenha] bit not null,
	[CREA] nvarchar(20) null,
	[CPF] nvarchar(20) null,
	[__Version] int not null CONSTRAINT DF_Usuario___Version DEFAULT 0
)

alter table [Usuario]
	add constraint [Usuario_PK] primary key ([UsuarioId])

create table [Estado]
(
	[EstadoID] int identity not null,
	[Sigla] nvarchar(2) not null CONSTRAINT [DF_Estado_Sigla] DEFAULT '',
	[Nome] nvarchar(100) not null CONSTRAINT [DF_Estado_Nome] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_Estado___Version DEFAULT 0
)

alter table [Estado]
	add constraint [Estado_PK] primary key ([EstadoID])

create table [Cidade]
(
	[CidadeID] int identity not null,
	[EstadoID] int not null,
	[Nome] nvarchar(150) not null CONSTRAINT [DF_Cidade_Nome] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_Cidade___Version DEFAULT 0
)

alter table [Cidade]
	add constraint [Cidade_PK] primary key ([CidadeID])

create table [Laudo]
(
	[LaudoID] int identity not null,
	[ImovelID] int not null,
	[ComarcaID] int null,
	[ResponsavelTecnicoID] int null,
	[RepresentanteLegalEmpresaID] int null,
	[ProdutoID] int null,
	[FonteID] int null,
	[LinhaID] int null,
	[Status] int not null,
	[DataHoraVistoria] datetime null,
	[UsosPredominantes] int not null,
	[FormaTerreno] int not null,
	[CotaGreideTerreno] int not null,
	[InclinacaoTerreno] int not null,
	[SituacaoTerreno] int not null,
	[SuperficieTerreno] int not null,
	[MedidaAreaTerreno] decimal(7,2) not null,
	[MedidaFrenteTerreno] decimal(7,2) not null,
	[MedidaFundosTerreno] decimal(7,2) not null,
	[MedidaDireitaTerreno] decimal(7,2) not null,
	[MedidaEsquerdaTerreno] decimal(7,2) not null,
	[FracaoIdealTerreno] float not null,
	[TipoEdificacao] int not null,
	[UsoEdificacao] int not null,
	[NumeroPavimentos] int not null,
	[IdadeEdificio] int not null,
	[PosicaoEdificacao] int not null,
	[PadraoAcabamento] int not null,
	[EstadoConservacao] int not null,
	[Tetos] int not null,
	[FechamentoParedes] int not null,
	[NumeroVagasCobertas] int not null,
	[NumeroVagasDescobertas] int not null,
	[AreaUnidadePrivativa] decimal(7,2) not null,
	[AreaUnidadeComum] decimal(7,2) not null,
	[AreaUnidadeTotal] decimal(7,2) not null,
	[AreaEstacionamentoPrivativa] decimal(7,2) not null,
	[AreaEstacionamentoComum] decimal(7,2) not null,
	[AreaEstacionamentoTotal] decimal(7,2) not null,
	[AreaOutrosPrivativa] decimal(7,2) not null,
	[AreaOutrosComum] decimal(7,2) not null,
	[AreaOutrosTotal] decimal(7,2) not null,
	[AreaTotalPrivativa] decimal(7,2) not null,
	[AreaTotalComum] decimal(7,2) not null,
	[AreaTotalAverbada] decimal(7,2) not null,
	[AreaTotalNaoAverbada] decimal(7,2) not null,
	[SomatorioAreas] decimal(7,2) not null,
	[NumeroQuartos] int not null,
	[NumeroSalas] int not null,
	[NumeroCirculacao] int not null,
	[NumeroBanheiros] int not null,
	[NumeroSuites] int not null,
	[NumeroCopas] int not null,
	[NumeroCozinhas] int not null,
	[NumeroAreasServico] int not null,
	[NumeroVarandas] int not null,
	[NumeroTerracosCobertos] int not null,
	[NumeroTerracosDescobertos] int not null,
	[UsoPredio] int not null,
	[NumeroPavimentosPredio] int not null,
	[NumeroElevadoresPredio] int not null,
	[PosicaoPredio] int not null,
	[PadraoConstrutivoPredio] int not null,
	[EstadoConservacaoPredio] int not null,
	[IdentificacaoPavimentosPredio] nvarchar(255) null,
	[IdadeAparentePredio] int not null,
	[ValorAvaliacao] decimal(17,2) not null,
	[ValorAvaliacaoExtenso] nvarchar(255) null,
	[MetodoDefinicaoValor] int not null,
	[AreaGlobal] decimal(7,2) not null,
	[ValorMetroQuadradoGlobal] decimal(7,2) not null,
	[ValorTotalGlobal] decimal(17,2) not null,
	[AreaTerreno] decimal(7,2) not null,
	[AreaEdificacao] decimal(7,2) not null,
	[AreaBenfeitorias] decimal(7,2) not null,
	[ValorMetroQuadradoTerreno] decimal(7,2) not null,
	[ValorMetroQuadradoEdificacao] decimal(7,2) not null,
	[ValorMetroQuadradoBenfeitorias] decimal(7,2) not null,
	[ProdutoTerreno] decimal(7,2) not null,
	[ProdutoEdificacao] decimal(7,2) not null,
	[ProdutoBenfeitorias] decimal(7,2) not null,
	[ValorTotalItemizada] decimal(17,2) not null,
	[PrecisaoFundamentacao] nvarchar(255) null,
	[MetodologiaAvaliacao] int not null,
	[DesempenhoMercado] int not null,
	[AbsorcaoMercado] int not null,
	[NivelOfertas] int not null,
	[NivelDemanda] int not null,
	[ObservacoesAvaliacao] nvarchar(255) null,
	[EstabilidadeSolidez] bit not null,
	[EstabilidadeSolidezJustificativa] nvarchar(255) null,
	[ViciosConstrucao] bit not null,
	[ViciosConstrucaoRelacao] nvarchar(255) null,
	[Habitabilidade] bit not null,
	[HabitabilidadeJustificativa] nvarchar(255) null,
	[FatoresLiquidezValorImovel] int not null,
	[AceitoComoGarantia] int not null,
	[MatriculaRGI] nvarchar(255) null,
	[Oficio] nvarchar(255) null,
	[OutrosDocumentos] nvarchar(255) null,
	[Conformidade] int not null,
	[Divergencia] nvarchar(255) null,
	[ObservacoesFinais] nvarchar(255) null,
	[LocalEmissaoLaudo] nvarchar(255) null,
	[Referencia] nvarchar(255) null,
	[NumeroClosets] int not null,
	[NumeroUnidadesPredio] int not null,
	[Solicitante] nvarchar(255) not null CONSTRAINT [DF_Laudo_Solicitante] DEFAULT '',
	[FatoresLiquidezExplicitacao] nvarchar(255) null,
	[__Version] int not null CONSTRAINT DF_Laudo___Version DEFAULT 0
)

alter table [Laudo]
	add constraint [Laudo_PK] primary key ([LaudoID])

create table [TipoLogradouro]
(
	[TipoLogradouroID] int identity not null,
	[Sigla] nvarchar(4) not null CONSTRAINT [DF_TipoLogradouro_Sigla] DEFAULT '',
	[Descricao] nvarchar(50) not null CONSTRAINT [DF_TipoLogradouro_Descricao] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_TipoLogradouro___Version DEFAULT 0
)

alter table [TipoLogradouro]
	add constraint [TipoLogradouro_PK] primary key ([TipoLogradouroID])

create table [Configuracao]
(
	[ConfiguracaoId] int identity not null,
	[NomeEmpresa] nvarchar(255) null,
	[CNPJEmpresa] nvarchar(255) null,
	[__Version] int not null CONSTRAINT DF_Configuracao___Version DEFAULT 0
)

alter table [Configuracao]
	add constraint [Configuracao_PK] primary key ([ConfiguracaoId])

create table [Produto]
(
	[ProdutoID] int identity not null,
	[Descricao] nvarchar(255) not null CONSTRAINT [DF_Produto_Descricao] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_Produto___Version DEFAULT 0
)

alter table [Produto]
	add constraint [Produto_PK] primary key ([ProdutoID])

create table [Fonte]
(
	[FonteID] int identity not null,
	[Descricao] nvarchar(255) not null CONSTRAINT [DF_Fonte_Descricao] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_Fonte___Version DEFAULT 0
)

alter table [Fonte]
	add constraint [Fonte_PK] primary key ([FonteID])

create table [Linha]
(
	[LinhaID] int identity not null,
	[Descricao] nvarchar(255) not null CONSTRAINT [DF_Linha_Descricao] DEFAULT '',
	[__Version] int not null CONSTRAINT DF_Linha___Version DEFAULT 0
)

alter table [Linha]
	add constraint [Linha_PK] primary key ([LinhaID])

-- Chaves estrangeiras:
alter table [Imovel] add constraint [FK_Imovel_Cidade] foreign key ([CidadeID]) references [Cidade] ([CidadeID])
alter table [Imovel] add constraint [FK_Imovel_TipoLogradouro] foreign key ([TipoLogradouroID]) references [TipoLogradouro] ([TipoLogradouroID])
alter table [ServicoPublicoComunitario] add constraint [FK_ServicoPublicoComunitario_Laudo] foreign key ([LaudoID]) references [Laudo] ([LaudoID])
alter table [InfraEstruturaUrbana] add constraint [FK_InfraEstruturaUrbana_Laudo] foreign key ([LaudoID]) references [Laudo] ([LaudoID])
alter table [Cidade] add constraint [FK_Cidade_Estado] foreign key ([EstadoID]) references [Estado] ([EstadoID])
alter table [Laudo] add constraint [FK_Laudo_Imovel] foreign key ([ImovelID]) references [Imovel] ([ImovelID])
alter table [Laudo] add constraint [FK_Laudo_Comarca] foreign key ([ComarcaID]) references [Cidade] ([CidadeID])
alter table [Laudo] add constraint [FK_Laudo_ResponsavelTecnico] foreign key ([ResponsavelTecnicoID]) references [Usuario] ([UsuarioId])
alter table [Laudo] add constraint [FK_Laudo_RepresentanteLegalEmpresa] foreign key ([RepresentanteLegalEmpresaID]) references [Usuario] ([UsuarioId])
alter table [Laudo] add constraint [FK_Laudo_Produto] foreign key ([ProdutoID]) references [Produto] ([ProdutoID])
alter table [Laudo] add constraint [FK_Laudo_Fonte] foreign key ([FonteID]) references [Fonte] ([FonteID])
alter table [Laudo] add constraint [FK_Laudo_Linha] foreign key ([LinhaID]) references [Linha] ([LinhaID])

-- Índices:
create index IX_ServicoPublicoComunitario_LaudoID on [ServicoPublicoComunitario] ([LaudoID])
create index IX_InfraEstruturaUrbana_LaudoID on [InfraEstruturaUrbana] ([LaudoID])
create index IX_Laudo_Status on [Laudo] ([Status])
create index IX_Cidade_Estado on [Cidade] ([EstadoID])
create index IX_Usuario_Email on [Usuario] ([Email])