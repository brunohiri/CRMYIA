$(document).ready(function () {
    var chat = new signalR.HubConnectionBuilder().withUrl("/notificacaohub").build();
   
    chat.start().then(function () {
        console.log('SignalR Started...')
        viewModel.userList();
    }).catch(function (err) {
        return console.error(err);
    });

    //chat.serverTimeoutInMilliseconds = 1000 * 60 * 10; // 1 second * 60 * 10 = 10 minutes.
    chat.on("NovaMensagemCaller", function (messageView) {
            var e_meu = messageView.nome === viewModel.meuNome();
            var mensagem = new ChatMensagem(messageView.nome, messageView.mensagem, messageView.dataCadastro, messageView.de, messageView.para, messageView.imagem, e_meu);
            viewModel.chatMensagem.push(mensagem);
        $(".chat-body").animate({ scrollTop: $(".chat-body")[0].scrollHeight }, 250);
        console.log('Meu')
    });

    chat.on("NovaMensagem", function (messageView) {
        var notificar = true;
        if ((messageView.para == $('#IdUsuario').val()) && ($('.direct-chat').hasClass('chat' + messageView.de))) {
            var e_meu = messageView.nome === viewModel.meuNome();
            var mensagem = new ChatMensagem(messageView.nome, messageView.mensagem, messageView.dataCadastro, messageView.de, messageView.para, messageView.imagem, e_meu);
            viewModel.chatMensagem.push(mensagem);
            notificar = false;
            $(".chat-body").animate({ scrollTop: $(".chat-body")[0].scrollHeight }, 250);
            //let AudioReceberMensagem = document.getElementById('receber-mensagem');
            //AudioReceberMensagem.play();

            //var AudioReceberMensagem = $('#receber-mensagem');
            //AudioReceberMensagem.trigger('load');

            var u = document.getElementById("receber-mensagem");

            function playChatSound() {
                u.play();
            };
            playChatSound();
        } 

        if (notificar && messageView.para == $('#IdUsuario').val()) {
            chat.invoke("NotificarMensagem", messageView.nome, messageView.mensagem, messageView.dataCadastro, messageView.de, messageView.para, messageView.imagem).catch(function (err) {
                return console.log(err.toString());
            });
        }
    });
    

    chat.on("getProfileInfo", function (meuNome, minhaImagem) {
        viewModel.meuNome(meuNome);
        viewModel.minhaImagem(minhaImagem);
    });

    chat.on("ReceberNotificacaoMensagem", function (dados) {
        //let audio = document.getElementById('notificacao');
        //audio.play();
        var html = '';
        if (dados.length == 0 && notificacaoMensagemVazia) {
            html = '';
            html = '<div class="dropdown-menu dropdown-menu-lg dropdown-menu-right show">\
                    <span class="dropdown-item dropdown-header"> 0 Mensagens</span>\
                        <div class="dropdown-divider"></div>\
                        <a href="/ListarNotificacao" class="dropdown-item dropdown-footer">Ver todas as mensagens</a>\
                        <div class="dropdown-divider"></div></div>';
            $('#notificacao-mensagem').html(html);
            notificacaoMensagemVazia = false;
        }
        else if (dados.length == 0 && $("#IdUsuario").val() == dados[0].para) {
            html = '';
            html = '<div class="dropdown-menu dropdown-menu-lg dropdown-menu-right show">\
                    <span class="dropdown-item dropdown-header"> 0 Mensagens</span>\
                        <div class="dropdown-divider"></div>\
                        <a href="/ListarNotificacao" class="dropdown-item dropdown-footer">Ver todas as mensagens</a>\
                        <div class="dropdown-divider"></div></div>';
            $('#notificacao-mensagem').html(html);
        }
       else if ($("#IdUsuario").val() == dados[0].para) {
            var notificacaoMensagem = dados.length;

            if (notificacaoMensagem > qtdNotificacaoMensagem) {
                qtdNotificacaoMensagem = notificacaoMensagem;
                //let AudioNotificacao = document.getElementById('notificacao');
                //AudioNotificacao.play();

                //var AudioNotificacao = $('#notificacao');
                //AudioNotificacao.trigger('load');

                //var v = document.getElementById("notificacao");

                //function playChatSound() {
                //    v.play();
                //};
                //playChatSound();
            } 
            if (!$('.notificacao-mensagem').hasClass('show')) {


                $.each(dados, function () {
                    if ($("#IdUsuario").val() == this.para) {

                        html += '<a href="#" class="dropdown-item">\
                                            <div class="media">\
                                                <img src="'+ this.imagem + '" alt="User Avatar" class="img-size-50 mr-3 img-circle">\
                                                    <div class="media-body">\
                                                        <h3 class="dropdown-item-title">'
                            + this.nome +
                            '<span class="float-right text-sm text-danger"><i class="fas fa-star"></i></span>\
                                                        </h3>\
                                                        <p class="text-sm">' + this.mensagem + '</p>\
                                                        <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i>' + this.dataCadastro + '</p>\
                                                    </div>\
                                        </div>\
                                    </a>\
                                    <div class="dropdown-divider"></div>';

                    }
                });
                html += '<div class="dropdown-divider"></div><a href="/ListarNotificacao" class="dropdown-item dropdown-footer">Ver todas as notificações</a><div class="dropdown-divider"></div>';
                $('.quantidade-notificacao-mensagem').html(qtdNotificacaoMensagem);
                $('#notificacao-mensagem').html(html);

            }
        }
        
    });

     setInterval(function () {
        let Id = $("#IdUsuario").val();
        connection.invoke("NotificacaoMensagemHub", Id).catch(function (err) {
            return console.log(err.toString());
        });
    }, 500);

    function AppViewModel() {
        var self = this;
        var allBindingsAccessor = $('#lista-usuario');
        self.idUsuario = ko.observable($("#IdUsuario").val());
        self.mensagem = ko.observable("");
        self.chatUsers = ko.observableArray([]);
        self.chatMensagem = ko.observableArray([]);
        self.chatMensagem = ko.observableArray([]);
        self.numeroDeLinha = ko.observable($("#NumeroDeLinha").val());
        self.pesquisaNome = ko.observable("");
        self.limite = ko.observable($("#Limite").val());
        self.serverInfoMessage = ko.observable("");
        self.meuNome = ko.observable("");
        self.minhaImagem = ko.observable("avatar1.png");
        self.onEnter = function (d, e) {
            if (e.keyCode === 13) {
                self.sendNewMessage();
            }
            return true;
        }
        self.filter = ko.observable("");

        self.filteredChatUsers = ko.observableArray([]);
        self.barraListaUsuario = ko.observable();

        self.para = ko.observable(null);
        self.buscarContato = function (item) {
            self.para(item);
            
            $('.direct-chat').addClass('chat' + self.para().idUsuario());
            $('.direct-chat').removeClass('direct-chat-contacts-open');
            SelectInputChat();
            $('#De').val(self.idUsuario());
            $('#Para').val(self.para().idUsuario());
            $('#Nome').val(self.meuNome())
            chat.invoke("GetMessageHistory", String(self.para().idUsuario()), String(self.idUsuario())).then(function (result) {
                self.chatMensagem.removeAll();
                for (var i = 0; i < result.length; i++) {
                    var eMeu = result[i].de == self.idUsuario();
                    self.chatMensagem.push(new ChatMensagem(
                        result[i].nome,
                        result[i].mensagem,
                        result[i].dataCadastro,
                        result[i].de,
                        result[i].para,
                        result[i].imagem,
                        eMeu
                    ));
                }
            });

            setTimeout(function () { $(".chat-body").animate({ scrollTop: $(".chat-body")[0].scrollHeight }, 250); }, 500)
        };

        //null
        self.eNulo = function (item) {
            self.para(item);
            console.log(self.para.mensagem(), self.para.dataMensagem());
            if (self.para.mensagem() == null && self.para.dataMensagem == null) {
                console.log(false);
                return false;
            }
                
            else
                return true
        };

        self.scrollListUsuario = function () {
            if ($('#lista-usuario').scrollTop() > $('#lista-usuario')[0].offsetHeight) {
                alert($('#lista-usuario')[0].offsetHeight)
            }
            
        };

        self.filteredChatUsers = ko.computed(function () {
            if (!self.filter()) {
                return self.chatUsers();
            } else {
                return ko.utils.arrayFilter(self.chatUsers(), function (user) {
                    var displayName = user.displayName().toLowerCase();
                    return displayName.includes(self.filter().toLowerCase());
                });
            }
        }); 

        self.pesquisarNomeUsuario = function (form, event) { 
            event.stopPropagation();
            self.userList();
        }

        self.enviarMensagem = function (form, event) {
            event.stopPropagation();
            chat.invoke('EnviarPrivado', String($('#De').val()), String($('#Para').val()), String($('#Nome').val()), String($('#Mensagem').val()), String(self.minhaImagem()))
                .catch(function (err) {
                return console.error(err.toString());
            });
            $('#Mensagem').val('')
        }

        self.userList = function () {
            let NumeroDeLinha = $('#NumeroDeLinha').val();
            let Limite = self.limite();
            let IdUsuario = $('#IdUsuario').val();
            chat.invoke('CarregarUsuarios', self.pesquisaNome()).then(function (result) {
                self.chatUsers.removeAll();
                for (var i = 0; i < result.length; i++) {
                    self.chatUsers.push(new ChatUser(
                        result[i].idUsuario,
                        result[i].nome,
                        result[i].imagem,
                        result[i].mensagem,
                        result[i].dataMensagem,
                    ))
                }
            });
        }

        
    }

    self.userAdded = function (user) {
        self.chatUsers.push(user);
    }

    self.userRemoved = function (id) {
        var temp;
        ko.utils.arrayForEach(self.chatUsers(), function (user) {
            if (user.userName() == id)
                temp = user;
        });
        self.chatUsers.remove(temp);
    }

   
    function ChatUser(idUsuario, nome, imagem, mensagem, dataMensagem) {
        var self = this;
        self.idUsuario = ko.observable(idUsuario);
        self.nome = ko.observable(nome);
        self.imagem = ko.observable(imagem);
        self.mensagem = ko.observable(mensagem);
        self.dataMensagem = ko.observable(dataMensagem);
    }

    function ChatMensagem(nome, mensagem, dataCadastro, de, para, imagem, eMeu) {
        var self = this;
        self.nomePessoaChat = ko.observable(nome);
        self.mensagemPessoaChat = ko.observable(mensagem);
        self.dataCadastroPessoaChat = ko.observable(dataCadastro);
        self.dePessoaChat = ko.observable(de);
        self.paraPessoaChat = ko.observable(para);
        self.imagemPessoaChat = ko.observable(imagem);
        self.eMeuPessoaChat = ko.observable(eMeu);
    }

    var viewModel = new AppViewModel();
    ko.applyBindings(viewModel);
    $(".btn-contato").on('click', function () {
        SelectInputChat();
    });
    
});

function SelectInputChat() {
    if ($("#pesquisar").hasClass("d-none")) {
        $("#pesquisar").removeClass('d-none');
    } else {
        $("#pesquisar").addClass('d-none');
    }

    if ($("#mensagem").hasClass("d-none")) {
        $("#mensagem").removeClass('d-none');
    } else {
        $("#mensagem").addClass('d-none');
    }
}