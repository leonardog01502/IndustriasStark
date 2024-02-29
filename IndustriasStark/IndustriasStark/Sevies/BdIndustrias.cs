using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IndustriasStark.Models;
using SQLite;
using Xamarin.Forms;

namespace IndustriasStark.Sevies
{
    public class BdIndustrias
    {
        SQLiteConnection conn;
        public string StatusMessage { get; set; }

        public BdIndustrias(string dbPath)
        {
            if (dbPath == "") dbPath = App.DbPath;
            conn = new SQLiteConnection(dbPath);
            conn.CreateTable<ModIndustrias>();
        }

        public void Inserir(ModIndustrias funcionario, byte[] foto)
        {
            try
            {
                if (string.IsNullOrEmpty(funcionario.Nome))
                    throw new Exception("Nome do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.CPF))
                    throw new Exception("CPF do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Telefone))
                    throw new Exception("Telefone do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Endereco))
                    throw new Exception("Endereço do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Bairro))
                    throw new Exception("Bairro do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Complemento))
                    throw new Exception("Complemento do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Email))
                    throw new Exception("E-mail do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Setor))
                    throw new Exception("Setor do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Turno))
                    throw new Exception("Turno do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Dados))
                    throw new Exception("Dados do funcionario não informado !");
                funcionario.Foto = foto;

                int result = conn.Insert(funcionario);
                if (result != 0)
                {
                    this.StatusMessage = string.Format("{0} Registros adicionados: [Funcionario: {1}]", result, funcionario.Nome);
                }
                else
                {
                    this.StatusMessage = string.Format("0 Registros adicionado ! Por favor, informe todas as informações do funcionario !");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ModIndustrias> Listar()
        {
            List<ModIndustrias> Lista = new List<ModIndustrias>();
            try
            {
                Lista = conn.Table<ModIndustrias>().ToList();
                this.StatusMessage = "Listagem dos Funcionários";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Lista;
        }

        public void Alterar(ModIndustrias funcionario)
        {
            try
            {
                if (funcionario.Id <= 0)
                    throw new Exception("ID do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Nome))
                    throw new Exception("Nome do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.CPF))
                    throw new Exception("CPF do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Telefone))
                    throw new Exception("Telefone do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Endereco))
                    throw new Exception("Endereço do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Bairro))
                    throw new Exception("Bairro do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Complemento))
                    throw new Exception("Complemento do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Email))
                    throw new Exception("E-mail do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Setor))
                    throw new Exception("Setor do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Turno))
                    throw new Exception("Turno do funcionario não informado !");
                if (string.IsNullOrEmpty(funcionario.Dados))
                    throw new Exception("Dados do funcionario não informado !");

                ModIndustrias funcionarioExistente = conn.Table<ModIndustrias>().FirstOrDefault(f => f.Id == funcionario.Id);

                funcionarioExistente.Nome = funcionario.Nome;
                funcionarioExistente.CPF = funcionario.CPF;
                funcionarioExistente.Telefone = funcionario.Telefone;
                funcionarioExistente.Endereco = funcionario.Endereco;
                funcionarioExistente.Bairro = funcionario.Bairro;
                funcionarioExistente.Complemento = funcionario.Complemento;
                funcionarioExistente.Email = funcionario.Email;
                funcionarioExistente.Setor = funcionario.Setor;
                funcionarioExistente.Turno = funcionario.Turno;
                funcionarioExistente.Dados = funcionario.Dados;

                int result = conn.Update(funcionarioExistente);

                if (result != 0)
                {
                    this.StatusMessage = string.Format("{0} Registro alterado: [Funcionario: {1}]", result, funcionarioExistente.Nome);
                }
                else
                {
                    this.StatusMessage = string.Format("0 Registro alterado ! Não foi possível atualizar o funcionario com ID {0}.", funcionario.Id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("ID do funcionario não informado !");

                ModIndustrias funcionarioExistente = conn.Table<ModIndustrias>().FirstOrDefault(f => f.Id == id);

                if (funcionarioExistente == null)
                    throw new Exception($"Funcionario com ID {id} não encontrado !");

                int result = conn.Delete(funcionarioExistente);

                if (result != 0)
                {
                    this.StatusMessage = string.Format("{0} Registro excluído: [Funcionario: {1}]", result, funcionarioExistente.Nome);
                }
                else
                {
                    this.StatusMessage = string.Format("0 Registro excluído ! Não foi possível excluir o funcionario com ID {0}.", id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ModIndustrias> Localizar(string cpf)
        {
            try
            {
                List<ModIndustrias> funcionariosEncontrados = conn.Table<ModIndustrias>().Where(f => f.CPF == cpf).ToList();

                if (funcionariosEncontrados.Count != 0)
                {
                    this.StatusMessage = string.Format("{0} Funcionário(s) encontrado(s) com o CPF {1}", funcionariosEncontrados.Count, cpf);
                }
                else
                {
                    this.StatusMessage = string.Format("Nenhum funcionário encontrado com o CPF {0}.", cpf);
                }

                return funcionariosEncontrados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ModIndustrias> Localizar(string cpf, bool favorito)
        {
            try
            {
                List<ModIndustrias> funcionariosEncontrados = conn.Table<ModIndustrias>().Where(f => f.CPF == cpf && f.Favorito == favorito).ToList();

                if (funcionariosEncontrados.Count != 0)
                {
                    this.StatusMessage = string.Format("{0} Funcionário(s) encontrado(s) com o CPF {1} e favorito: {2}", funcionariosEncontrados.Count, cpf, favorito);
                }
                else
                {
                    this.StatusMessage = string.Format("Nenhum funcionário encontrado com o CPF {0} e favorito: {1}.", cpf, favorito);
                }

                return funcionariosEncontrados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ModIndustrias GetFuncionarios(int id)
        {
            try
            {
                ModIndustrias funcionario = conn.Table<ModIndustrias>().FirstOrDefault(f => f.Id == id);

                if (funcionario != null)
                {
                    this.StatusMessage = string.Format("Funcionário encontrado com o  {0}.", id);
                }
                else
                {
                    this.StatusMessage = string.Format("Funcionário não encontrado com o ID {0}.", id);
                }

                return funcionario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string FormatarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return string.Empty;

            var cleanCpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cleanCpf.Length != 11) return cleanCpf;

            return $"{cleanCpf.Substring(0, 3)}.{cleanCpf.Substring(3, 3)}.{cleanCpf.Substring(6, 3)}-{cleanCpf.Substring(9, 2)}";
        }
    }
}
