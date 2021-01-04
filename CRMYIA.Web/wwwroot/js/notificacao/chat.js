$(document).ready(function () {
    var ScrollListaUsuario = 250;
    var scrollAltoBaixo = true;
    var chat = new signalR.HubConnectionBuilder().withUrl("/notificacaohub").build();

    chat.start().then(function () {
        console.log('SignalR Started...')
        //viewModel.roomList();
        viewModel.userList();
        //setTimeout(function () {
        //    if (viewModel.chatRooms().length > 0) {
        //        viewModel.joinRoom(viewModel.chatRooms()[0]);
        //    }
        //}, 250);
    }).catch(function (err) {
        return console.error(err);
    });

    //chat.serverTimeoutInMilliseconds = 1000 * 60 * 10; // 1 second * 60 * 10 = 10 minutes.

    chat.on("newMessage", function (messageView) {
        var e_meu = messageView.de === viewModel.meuNome();
        var mensagem = new ChatMensagem(messageView.conteudo, messageView.dataCadastro, messageView.de, e_meu, messageView.imagem);
        viewModel.chatMensagem.push(mensagem);
        $(".chat-body").animate({ scrollTop: $(".chat-body")[0].scrollHeight }, 1000);
    });

    connection.on("getProfileInfo", function (meuNome, minhaImagem) {
        viewModel.meuNome(meuNome);
        viewModel.minhaImagem(minhaImagem);
    });

    function AppViewModel() {
        var self = this;
        var allBindingsAccessor = $('#lista-usuario');
        self.idUsuario = ko.observable($("#IdUsuario").val());
        self.mensagem = ko.observable("");
        //self.chatRooms = ko.observableArray([]);
        self.chatUsers = ko.observableArray([]);
        self.chatMensagem = ko.observableArray([]);
        //self.joinedRoom = ko.observable("");
        //self.joinedRoomId = ko.observable("");
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

        //scroll

        self.filteredChatUsers = ko.observableArray([]);
        self.barraListaUsuario = ko.observable();

        self.para = ko.observable(null);
        self.buscarContato = function (item) {
            self.para(item);
            console.log(self.para().idUsuario(), self.para().nome());
            $('.direct-chat').removeClass('direct-chat-contacts-open');
            chat.invoke("SendPrivate", String(self.para().idUsuario()), String(self.para().nome()));
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

        self.sendNewMessage = function () {
            var text = self.message();
            if (text.startsWith("/")) {
                var receiver = text.substring(text.indexOf("(") + 1, text.indexOf(")"));
                var message = text.substring(text.indexOf(")") + 1, text.length);

                if (receiver.length > 0 && message.length > 0) {
                  //connection.invoke("SendPrivate", receiver.trim(), message.trim());
                }    
            }
            else {
                if (self.joinedRoom().length > 0 && self.message().length > 0)
                    connection.invoke("SendToRoom", self.joinedRoom(), self.message());
            }

            self.message("");
        }  

        self.pesquisarNomeUsuario = function (form, event) {
            event.stopPropagation();
            self.userList();
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

    var viewModel = new AppViewModel();
    ko.applyBindings(viewModel);
    //bindScrollHandler();
    $(".btn-contato").on('click', function () {
        SelectInputChat();
    });
    
});

//$(document).on('click', '.idusuario-para', function () {
//    $('.direct-chat').removeClass('direct-chat-contacts-open');
//    SelectInputChat();
//   $('#Para').val($(this).data('idusuario'));
    
//    console.log(ko);
//});

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

//var bindScrollHandler = function () {
//    $("#lista-usuario").scroll(function () {
//        if ($("#lista-usuario").scrollTop() + $("#lista-usuario").height() > $(document).height() - 200) {
//            console.log('foi');
//        }
//    });
//};