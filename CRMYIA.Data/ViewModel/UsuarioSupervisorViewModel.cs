﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class UsuarioSupervisorViewModel
    {
        public long IdUsuario { get; set; }
        public string Nome { get; set; }
        public string NomeApelido { get; set; }
        public List<UsuarioCorretorViewModel> UsuariosCorretores { get; set; }
    }
}
