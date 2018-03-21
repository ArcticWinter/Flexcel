using System;
using System.ComponentModel;
using System.Windows;
using Domain;
using System.Linq;
using System.Collections.Generic;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnMasterDataFilePathSelect_Click(object sender, RoutedEventArgs e)
        {
            txtBoxFilePathMasterData.Text = mainWindowViewModel.ChooseCSVFile();
        }
        private void BtnRouteNumberFilePathSelect_Click(object sender, RoutedEventArgs e)
        {
            txtBoxFilePathRouteNumberOffer.Text = mainWindowViewModel.ChooseCSVFile();
        }
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBoxFilePathMasterData.Text.ToString().Equals("") || txtBoxFilePathRouteNumberOffer.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Vælg venligst begge filer inden import startes");
                }
                else if ((txtBoxFilePathMasterData.Text.ToString().Equals("") && txtBoxFilePathRouteNumberOffer.Text.ToString().Equals("")))
                {
                    MessageBox.Show("Vælg venligst filerne inden import startes");
                }
                else
                {
                    mainWindowViewModel.ImportCSV(txtBoxFilePathMasterData.Text.ToString(), txtBoxFilePathRouteNumberOffer.Text.ToString());
                    MessageBox.Show("Filerne er nu importeret");
                    mainWindowViewModel.ImportDone = true;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

        }
        private void BtnStartSelection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindowViewModel.InitializeSelection(); // Checks if the 2 .CSV files has been imported, if so, selects the winners.
                ListContainer listContainer = ListContainer.GetInstance(); // No clue what this does
                List<Offer> outputListByUserID = listContainer.outputList.OrderBy(x => x.UserID).ToList(); // Sorts the list
                listView.ItemsSource = outputListByUserID; // Updates the GUI with the results
                foreach(Offer offer in listContainer.outputList) // No clue what this does
                {
                    offer.Contractor.CountNumberOfWonOffersOfEachType(listContainer.outputList); 
                }
                MessageBox.Show("Udvælgelsen er nu færdig"); // Shows a pop message telling the selection has been completed
            }
            catch (Exception x) // Error handling
            {
                PromptWindow promptWindow = new PromptWindow(x.Message);
                promptWindow.Show();
            }
        }
      
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Environment.Exit(1);
        }

        private void btnSavePublic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindowViewModel.SaveCSVPublishFile();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void btnSaveCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindowViewModel.SaveCSVCallFile();
            }          
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
    }
}
