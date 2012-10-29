--Generated using version 1.0 of the NeedhamGroupDSL Tool

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

create index IX_ServicoPublicoComunitario_LaudoID on [ServicoPublicoComunitario] ([LaudoID])
create index IX_InfraEstruturaUrbana_LaudoID on [InfraEstruturaUrbana] ([LaudoID])
