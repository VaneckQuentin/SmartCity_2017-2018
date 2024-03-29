﻿using BackOffice_SmartCity.Model;
using BackOffice_SmartCity.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace BackOffice_SmartCity.ViewModel
{
    public class UtilisateurViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private ApplicationUser _selectedUser;
        private ObservableCollection<ApplicationUser> _utilisateur = null;

        public ObservableCollection<ApplicationUser> Utilisateur
        {
            get
            {
                return _utilisateur;
            }

            set
            {
                if (_utilisateur == value)
                {
                    return;
                }
                _utilisateur = value;
                RaisePropertyChanged("Utilisateur");
            }
        }

        public ApplicationUser SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    RaisePropertyChanged("SelectedGarden");
                }
            }
        }

        public UtilisateurViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            InitializeAsync();
        }

        public ICommand NavigateToAcceuil
        {
            get
            {
                return new RelayCommand(() => GoToAcceuil());
            }
        }

        private void GoToAcceuil()
        {
            navigationService.GoBack();
        }

        public async Task InitializeAsync()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                AccountController service = new AccountController();
                IEnumerable<ApplicationUser> listeUti = await service.GetAllElements();
                Utilisateur = new ObservableCollection<ApplicationUser>(listeUti);
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog(Constantes.MESSAGE_ERREUR_CONNEXION)
                {
                    Title = Constantes.TITRE_ERREUR_CONNECTION
                };
                var res = messageDialog.ShowAsync();
            }
        }

        public ICommand DeleteUserCommand
        {
            get
            {
                return new RelayCommand(async () => await DeleteUser());
            }

        }

        public async Task DeleteUser()
        {
            if (CanExecute() && NetworkInterface.GetIsNetworkAvailable())
            {
                AccountController service = new AccountController();

                MessageDialog messageDialog = new MessageDialog(Constantes.MESSAGE_SUPPRESSION + SelectedUser.UserName + " ?")
                {
                    Title = Constantes.TITRE_SUPPRESSION
                };
                messageDialog.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
                messageDialog.Commands.Add(new UICommand { Label = "Non", Id = 1 });
                var res = await messageDialog.ShowAsync();

                if ((int)res.Id == 0)
                {
                    if(SelectedUser.UserName.Contains("Admin"))
                    {
                        MessageDialog messageDialogErreur = new MessageDialog(Constantes.MESSAGE_ERREUR_DEL_ADMIN)
                        {
                            Title = Constantes.TITRE_ERREUR_DEL_ADMIN
                        };
                        var show = await messageDialogErreur.ShowAsync();
                    }
                    else
                    {
                        Task delResp = service.DeleteUser(SelectedUser.UserName);
                        await InitializeAsync();
                    }                  
                }
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog(Constantes.MESSAGE_ERREUR_CONNEXION)
                {
                    Title = Constantes.TITRE_ERREUR_CONNECTION
                };
                var res = messageDialog.ShowAsync();
            }
        }

        private bool CanExecute()
        {
            return SelectedUser != null;
        }

        public ICommand Actualiser
        {
            get
            {
                return new RelayCommand(async () => await InitializeAsync());
            }
        }

    }
}
