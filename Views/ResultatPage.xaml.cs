namespace CalculateurAge.Views;

// Relie le paramètre "nom" de l'URL à la propriété Nom.
[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
public partial class ResultatPage : ContentPage
{
    // Remplies par la navigation, APRÈS le constructeur.
    public string Nom { get; set; }
    public string Age { get; set; }

    public ResultatPage() => InitializeComponent();

    protected override void OnAppearing()
    {
        base.OnAppearing();
        lblMessage.Text = $"{Nom}, vous avez {Age} ans";
    }

    private async void OnRetourClicked(object s, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
