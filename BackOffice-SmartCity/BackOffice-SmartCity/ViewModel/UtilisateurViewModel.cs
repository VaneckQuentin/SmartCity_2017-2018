﻿using BackOffice_SmartCity.Model;
using BackOffice_SmartCity.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace BackOffice_SmartCity.ViewModel
{
    public class UtilisateurViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private ApplicationUser _selectedUser;

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
                    return;                 //On ne le fait que si la valeur a change pour evite de passer au RaisePropertyChanged pour ne pas perturber les ecouteurs 
                }
                _utilisateur = value;
                RaisePropertyChanged("Utilisateur");
            }
        }

        public async Task InitializeAsync()
        {
            var service = new AccountController();
            var listeUti = await service.GetAllElements();
            Utilisateur = new ObservableCollection<ApplicationUser>(listeUti);
        }

        /*---DELETE---*/

        public ICommand DeleteUserCommand                                            //Le getter de "l'action event"
        {
            get
            {
                return new RelayCommand(() => DeleteUser());
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
                    RaisePropertyChanged("SelectedGarden");                            //Permet ici de détecter qu'un responsable a été coché dans la liste
                }
            }
        }

        public async Task DeleteUser()
        {
            if (CanExecute())
            {
                var service = new AccountController();
                var delResp = service.DeleteUser(SelectedUser.UserName);
            }
        }

        private bool CanExecute()
        {
            return SelectedUser != null;
        }

    }
}