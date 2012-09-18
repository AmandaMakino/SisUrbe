// SysCEF functions
SysCEF = {};

SysCEF.AbaAtual = '#Importado';
SysCEF.AjaxLoaderUrl = '';
SysCEF.CaminhoImportarOS = '';
SysCEF.CaminhoListaLaudos = '';
SysCEF.CaminhoCadastroUsuarios = '';
SysCEF.CaminhoBancoImoveis = '';
SysCEF.CaminhoConfiguracoes = '';
SysCEF.CancelImageUrl = '';
SysCEF.UploaderUrl = '';
SysCEF.CaminhoAcaoImportar = '';

SysCEF.ConfigurarLogin = function (caminhoGeracaoSenha) {
    $("#linksFooter").hide();

    $("input:submit").button();
    $("input:button").button();

    $("#loginEmail").focus();

    $("#linkEsqueceuSenha").click(function () {
        $("#resetBackground").css("display", "block");
        $("#resetBody").css("display", "block");
        $("#identificacaoEmail").focus();
    });

    $("#gerarNovaSenha").click(function () {
        var email = $("#identificacaoEmail").val();

        if (email.length == 0) {
            $("#resetEmailRequiredMessage").css("display", "block");
            $("#identificacaoEmail").addClass("input-validation-error");
            $("#identificacaoEmail").focus();
            return;
        }

        $.ajax({
            type: 'POST',
            url: caminhoGeracaoSenha + '?email=' + email,
            success: function (result) {
                $("#resetEmailRequiredMessage").css("display", "none");
                $("#identificacaoEmail").removeClass("input-validation-error");
                $(this).css("display", "none");
                $("#resetInfo").text(result);
                $("#cancelarCriacaoNovaSenha").val("Fechar");
            }
        });
    });

    $("#cancelarCriacaoNovaSenha").click(function () {
        $("#resetEmailRequiredMessage").css("display", "none");
        $("#identificacaoEmail").val("");
        $("#identificacaoEmail").removeClass("input-validation-error");
        $("#resetInfo").text("A sua senha temporária será criada e exibida no lugar dessa mensagem.");
        $("#gerarNovaSenha").css("display", "block");
        $("#cancelarCriacaoNovaSenha").val("Cancelar");
        $("#resetBackground").css("display", "none");
        $("#resetBody").css("display", "none");
    });
};

SysCEF.ConfigurarRedefinicaoSenha = function (mensagemErro) {
    $("input:submit").button();

    if (mensagemErro != "")
        alert(mensagemErro);
};

SysCEF.ConfigurarHome = function () {
    $("#linksFooter").show();
    $("#fecharDialogExportacaoBtn").button();

    if (window.location.href.indexOf('#') > 0) {
        SysCEF.AbaAtual = window.location.href.substring(window.location.href.indexOf('#'));
    }

    $("#tabs").tabs();

    $("#tabs").bind('tabsselect', function (event, ui) {
        window.location.href = ui.tab;
        SysCEF.AbaAtual = ui.tab.hash;

        var tabContentUrl = "";

        switch (SysCEF.AbaAtual) {
            case "#Importado":
                tabContentUrl = SysCEF.CaminhoImportarOS;
                break;
            case "#BancoImoveis":
                tabContentUrl = SysCEF.CaminhoBancoImoveis;
                break;
            case "#CadastroUsuarios":
                tabContentUrl = SysCEF.CaminhoCadastroUsuarios;
                break;
            case "#Configuracoes":
                tabContentUrl = SysCEF.CaminhoConfiguracoes;
                break;
            default:
                tabContentUrl = SysCEF.CaminhoListaLaudos + "?status=" + SysCEF.AbaAtual.replace('#', '');
                break;
        }

        $.ajax({
            type: 'POST',
            url: tabContentUrl,
            success: function (result) {
                $(SysCEF.AbaAtual).html(result);
            }
        });
    });
};

SysCEF.SelecionarAba = function(index) {
    $("#tabs").tabs('select', index);
};

SysCEF.ConfigurarUpload = function () {
    // Multiple files - single input
    var auth = "<% = Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value %>";
    var ASPSESSID = "<%= Session.SessionID %>";

    $("#fileuploader").fileUpload({
        'uploader': SysCEF.UploaderUrl,
        'cancelImg': SysCEF.CancelImageUrl,
        'buttonText': 'Importar OS...',
        'script': SysCEF.CaminhoAcaoImportar,
        'scriptData': { ASPSESSID: ASPSESSID, AUTHID: auth },
        'fileDataName': 'FileData',
        'multi': false,
        'sizeLimit': 200000000,
        'simUploadLimit': 1,
        'folder': '/Content/uploads',
        'fileDesc': 'Arquivos texto',
        'fileExt': '*.txt;',
        'auto': true,
        'onError': function (a, b, c, d) {
            if (d.status == 404)
                alert("Could not find upload script. Use a path relative to: " + "<?= getcwd() ?>");
            else if (d.type === "HTTP")
                alert("error " + d.type + ": " + d.status);
            else if (d.type === "File Size")
                alert(c.name + " " + d.type + " Limit: " + Math.round(d.info / (1024 * 1024)) + "MB");
            else
                alert("error " + d.type + ": " + d.text);
        },
        'onComplete': function(){

            $.ajax({
                type: 'GET',
                url: SysCEF.CaminhoListaLaudos,
                success: function (result) {
                    $("#listaOSs").html(result);

                    alert("Arquivo importado com sucesso!");
                }
            });
        }
    });
};

SysCEF.Editar = function (editarUrl, id) {
    $(SysCEF.AbaAtual).html("<div style='margin-left: 5px; min-height: 182px;'>" +
        "<p>Carregando ...<img src='" + SysCEF.AjaxLoaderUrl + "' alt = 'Por favor aguarde...  '/></p></div>");
        
    $.ajax({
        type: 'POST',
        url: editarUrl + '?id=' + id,
        success: function (result) {
            $(SysCEF.AbaAtual).html(result);
        }
    });
};

SysCEF.Exportar = function (exportarUrl, uploadsUrl) {
    $("#exportarBackground").css("display", "block");
    $("#exportarBody").css("display", "block");


    $.ajax({
        type: 'POST',
        url: exportarUrl,
        success: function (result) {

            $("#loadingExportacao").css("display", "none");

            $("#mensagemExportacao").text(result.mensagem);

            if (result.sucesso) {
                $("#mensagemDownload").css("display", "block");

                setTimeout(function() {
                    SysCEF.FazerDownload(uploadsUrl + result.nomeArquivo);
                }, 2000);
            }
        }
    });
};

SysCEF.FazerDownload = function (downloadUrl) {
    //    $("#linkDownload").attr("href", downloadUrl);
    //    $("#linkDownload").click();
    document.location.href = downloadUrl;

    // Esconde o dialog.
    $("#exportarBackground").css("display", "none");
    $("#exportarBody").css("display", "none");

    // Volta ao estado inicial.
    $("#mensagemExportacao").text("Exportando por favor aguarde ...");
    $("#loadingExportacao").css("display", "block");
    $("#mensagemDownload").css("display", "none");
};

SysCEF.ConfigurarFormLaudo = function (salvarLaudoUrl, atualizarAreasEdificacaoUrl) {
    $("#salvarLaudoBtn").button();

    $("#salvarLaudoBtn").click(function () {
        var servicoStr = "";
        var infraStr = "";
        $("#servicos_table INPUT[type='checkbox']").each(function () {
            servicoStr += this.id + "|" + this.checked + "&";
        });
        $("#infras_table INPUT[type='checkbox']").each(function () {
            infraStr += this.id + "|" + this.checked + "&";
        });

        $("#ServicosSelecionados").val(servicoStr);
        $("#InfrasSelecionadas").val(infraStr);

        $.ajax({
            type: 'POST',
            data: $('#formLaudo').serialize(),
            url: salvarLaudoUrl,
            success: function (result) {
                $(SysCEF.AbaAtual).html(result);
            }
        });
    });

    $('#dataVistoria').mask("99/99/9999", { placeholder: "_" });
    $('#horaVistoria').mask("99:99", { placeholder: "_" });

//    $('#areasEdificacao input').keyup(function () {
//        $.ajax({
//            type: 'POST',
//            data: $('#formLaudo').serialize(),
//            url: atualizarAreasEdificacaoUrl,
//            success: function (result) {
//                $('#areasEdificacao').html(result);
//            }
//        });
//    });
};

SysCEF.ConfigurarListaUsuario = function () {
    $("#adicionarUsuarioBtn").button();
    $(".mensagem").delay(1000).fadeOut("slow");
};

SysCEF.AdicionarUsuario = function(adicionarUsuarioUrl) {
    $(SysCEF.AbaAtual).html("<div style='margin-left: 5px; min-height: 182px;'>" +
        "<p>Carregando ...<img src='" + SysCEF.AjaxLoaderUrl + "' alt = 'Por favor aguarde...  '/></p></div>");

    $.ajax({
        type: 'GET',
        url: adicionarUsuarioUrl,
        success: function(result) {
            $(SysCEF.AbaAtual).html(result);
        }
    });
};

SysCEF.ExcluirUsuario = function(excluirUsuarioUrl, usuarioId) {
    if (confirm("Deseja mesmo excluir esse usuário?")) {
        $.ajax({
            type: 'GET',
            url: excluirUsuarioUrl + '?id=' + usuarioId,
            success: function (result) {
                $(SysCEF.AbaAtual).html(result);
            }
        });
    }
};

SysCEF.ConfigurarFormUsuario = function (salvarUsuarioUrl) {
    $("#salvarUsuarioBtn").button();
    $('#cpfUsuario').mask("999.999.999-99", { placeholder: "_" });
    
    $("#salvarUsuarioBtn").click(function () {
        var formValido = true;

        var id = $("#idUsuario").val();
        var nome = $("#nomeUsuario").val();
        var email = $("#emailUsuario").val();
        var senha = $("#senhaUsuario").val();
        var confirmacao = $("#confirmacaoUsuario").val();

        if (nome.length > 0) {
            $("#nomeUsuarioRequiredMessage").css("display", "none");
            $("#nomeUsuario").removeClass("input-validation-error");
        } else {
            formValido = false;
            $("#nomeUsuarioRequiredMessage").css("display", "block");
            $("#nomeUsuario").addClass("input-validation-error");
            $("#nomeUsuario").focus();
        }

        if (email.length > 0) {
            $("#emailUsuarioRequiredMessage").css("display", "none");
            $("#emailUsuario").removeClass("input-validation-error");
        } else {
            formValido = false;
            $("#emailUsuarioRequiredMessage").css("display", "block");
            $("#emailUsuario").addClass("input-validation-error");
        }

        if (senha.length === 0 || confirmacao.length === 0) 
        {
            if (senha.length > 0) {
                $("#senhaUsuarioRequiredMessage").css("display", "none");
                if (senha.length > 5) {
                    $("#minimoSenhaUsuarioMessage").css("display", "none");
                    $("#senhaUsuario").removeClass("input-validation-error");
                } else {
                    $("#minimoSenhaUsuarioMessage").css("display", "block");
                    $("#senhaUsuario").addClass("input-validation-error");
                }
            } else if (id === '0') { // Exibe mensgem obrigatória só se estiver criando
                formValido = false;
                $("#senhaUsuarioRequiredMessage").css("display", "block");
                $("#senhaUsuario").addClass("input-validation-error");
            }

            if (confirmacao.length > 0) {
                $("#confirmacaoUsuarioRequiredMessage").css("display", "none");
                $("#confirmacaoUsuario").removeClass("input-validation-error");
            } else if (id === '0') { // Exibe mensgem obrigatória só se estiver criando
                formValido = false;
                $("#confirmacaoUsuarioRequiredMessage").css("display", "block");
                $("#confirmacaoUsuario").addClass("input-validation-error");
            }
        }
        else if (senha !== confirmacao) {
            formValido = false;
            $("#divergenciaUsuarioMessage").css("display", "block");
            $("#confirmacaoUsuario").addClass("input-validation-error");
        } else {
            $("#divergenciaUsuarioMessage").css("display", "none");
            $("#confirmacaoUsuario").removeClass("input-validation-error");
        }

        if (formValido) {
            $.ajax({
                type: "POST",
                url: salvarUsuarioUrl,
                data: $("#formUsuario").serialize(),
                success: function (resultado) {
                    $(SysCEF.AbaAtual).html(resultado);
                }
            });
        }
    });
};


SysCEF.ConfigurarConfiguracoes = function (salvarConfiguracoesUrl) {
    $('#cnpjEmpresa').mask("99.999.999/9999-99", { placeholder: "_" });

    $("#salvarConfiguracoesBtn").button();
    $("#salvarConfiguracoesBtn").click(function () {
        $.ajax({
            type: "POST",
            url: salvarConfiguracoesUrl,
            data: $("#formConfiguracao").serialize(),
            success: function (resultado) {
                $("#formConfiguracao").find("#mensagem").html(resultado);
                $("#formConfiguracao").find("#mensagem").show();
                $("#formConfiguracao").find("#mensagem").delay(1000).fadeOut("slow");
            }
        });
    });
};