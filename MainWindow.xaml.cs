﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static DTCWaitingList.PacientesEmListaDeEspera;

namespace DTCWaitingList
{
    public partial class PacientesEmListaDeEspera : Window
    {
        // Definindo uma classe para representar uma entrada de paciente
        public class Paciente
        {
            public string Nome { get; set; }
            public string Apelido { get; set; }
            public string Email { get; set; }
            public string Telefone { get; set; }
            public string DiasDisponiveis { get; set; }
            public string HorasDisponíveis { get; set; }
            public string TipoConsulta { get; set; }
            public bool NovoPaciente { get; set; }
            public DateTime DataEntradaFilaEspera { get; set; }
        }

        // Lista de pacientes para armazenar os mocks
        public ObservableCollection<Paciente> Resultados { get; set; }

        public PacientesEmListaDeEspera()
        {
            InitializeComponent();
            Resultados = new ObservableCollection<Paciente>();
            // Define o contexto de dados para a ListView
            MockResultados();
            listView.ItemsSource = Resultados;

            DataContext = this;
        }

        // Método para adicionar mocks à lista de resultados



        private void MockResultados()
        {
            // Adiciona 3 entradas de exemplo
            Resultados.Add(new Paciente
            {
                Nome = "João",
                Apelido = "Silva",
                Email = "joao.silva@example.com",
                Telefone = "123456789",
                DiasDisponiveis = "Segunda-feira, Terça-feira",
                TipoConsulta = "Checkup",
                NovoPaciente = true,
                DataEntradaFilaEspera = DateTime.Now.AddDays(-5)
            });
            Resultados.Add(new Paciente
            {
                Nome = "Maria",
                Apelido = "Santos",
                Email = "maria.santos@example.com",
                Telefone = "987654321",
                DiasDisponiveis = "Quarta-feira, Sexta-feira",
                TipoConsulta = "Cleaning",
                NovoPaciente = false,
                DataEntradaFilaEspera = DateTime.Now.AddDays(-7)
            });
            Resultados.Add(new Paciente
            {
                Nome = "Carlos",
                Apelido = "Oliveira",
                Email = "carlos.oliveira@example.com",
                Telefone = "567891234",
                DiasDisponiveis = "Segunda-feira, Quarta-feira",
                HorasDisponíveis = "Manhã",
                TipoConsulta = "Checkup",
                NovoPaciente = true,
                DataEntradaFilaEspera = DateTime.Now.AddDays(-10)
            });
        }

        private void Procurar_Click(object sender, RoutedEventArgs e)
        {
            var resultadosFiltrados = new ObservableCollection<Paciente>();
            resultadosFiltrados = Resultados;

            // Verifica se os ComboBoxes têm valores selecionados
            if (cmbDiasDisponiveis.SelectedItem != null ||
                cmbHoraDisponivel.SelectedItem != null ||
                cmbTipoConsulta.SelectedItem != null)
            {
                // Resultados.Clear();
                //buscar resultados filtrados à DB, não esquecer "any day" e "any time". Por agora, mostra tudo

                resultadosFiltrados = Resultados;
            }

            listView.ItemsSource = resultadosFiltrados;
        }

        private void RemoverPaciente_Click(object sender, RoutedEventArgs e)
        {
            // Obtemos o botão que foi clicado
            Button button = sender as Button;
            // Obtemos o paciente associado à linha onde o botão foi clicado
            Paciente paciente = button.DataContext as Paciente;

            // Exibimos o popup de confirmação
            MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja remover {paciente.Nome} {paciente.Apelido} da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Se o usuário confirmar a remoção, removemos o paciente da lista
            if (result == MessageBoxResult.Yes)
            {
                Resultados.Remove(paciente);
            }
        }


    }


}
