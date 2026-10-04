namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    // ---------- Champs privés ----------
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private string _message = "";
    private bool _resultatVisible;
    private int _age;              // dernier âge calculé
    private string _nomCalcule = ""; // nom utilisé pour ce calcul

    // ---------- Propriétés publiques ----------
    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value))
                  CalculerCommand.Rafraichir(); }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set
        {
            if (SetField(ref _dateNaissance, value))
            {
                OnPropertyChanged(nameof(ErreurDate));
                OnPropertyChanged(nameof(ErreurVisible));
                CalculerCommand.Rafraichir();
            }
        }
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    // FONCTIONNALITÉ 1 : « Majeur » ou « Mineur »
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set { if (SetField(ref _resultatVisible, value))
                  OuvrirDetailCommand.Rafraichir(); }
    }

    // FONCTIONNALITÉ 3 : refus d'une date future
    private bool DateFuture => DateNaissance.Date > DateTime.Today;
    public bool ErreurVisible => DateFuture;
    public string ErreurDate => DateFuture
        ? "La date de naissance ne peut pas être dans le futur."
        : "";

    // ---------- Commandes ----------
    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }
    public RelayCommand OuvrirDetailCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom) && !DateFuture);

        // FONCTIONNALITÉ 2 : remise à zéro de tous les champs
        EffacerCommand = new RelayCommand(Effacer);

        OuvrirDetailCommand = new RelayCommand(
            async () => await OuvrirDetailAsync(),
            () => ResultatVisible);
    }

    // ---------- Logique métier (aucun contrôle d'interface) ----------
    private void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age)) age--;

        _age = age;
        _nomCalcule = Nom.Trim();

        Resultat = $"{_nomCalcule}, vous avez {age} ans";
        Message = age >= 18 ? "Majeur" : "Mineur";
        ResultatVisible = true;
    }

    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        Message = "";
        ResultatVisible = false;
    }

    // Navigation vers ResultatPage par l'URL (comme à la phase B).
    private async Task OuvrirDetailAsync()
    {
        await Shell.Current.GoToAsync(
            $"ResultatPage?nom={Uri.EscapeDataString(_nomCalcule)}&age={_age}");
    }
}
