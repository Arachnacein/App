namespace UI.Pages.MyPages;

public partial class Register
{
    [Inject] private IStringLocalizer<Register> Localizer {  get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private RegistrationViewModelValidator RegistrationModelValidator { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }
    private RegistrationViewModel _registrationModel = new RegistrationViewModel();
    private MudForm _form;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    private InputType _passwordInput = InputType.Password;
    private bool _isShow;

    private async Task Submit()
    {
        await _form.ValidateAsync();

        var recaptchaResponse = await JSRuntime.InvokeAsync<string>("grecaptcha.getResponse");
        if (string.IsNullOrEmpty(recaptchaResponse))
        {
            Snackbar.Add("Please complete the CAPTCHA", Severity.Error);
            return;
        }

        var isValidCaptcha = await ValidateCaptcha(recaptchaResponse);
        if (!isValidCaptcha)
        {
            Snackbar.Add("Invalid CAPTCHA", Severity.Error);
            return;
        }

        if (_form.IsValid)
        {
            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("firstname", _registrationModel.FirstName),
                new KeyValuePair<string, string>("lastname", _registrationModel.LastName),
                new KeyValuePair<string, string>("username", _registrationModel.Username),
                new KeyValuePair<string, string>("email", _registrationModel.Email),
                new KeyValuePair<string, string>("enabled", "true"),
                new KeyValuePair<string, string>("password", _registrationModel.Password),
            });


            var response = await HttpClient.PostAsync("/api/register", requestBody);

            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add(Localizer["RegisterSuccess"], Severity.Success);
                Navigation.NavigateTo("/", false);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
                Snackbar.Add(Localizer[$"Error_{errorResponse.ErrorCode}"], Severity.Warning);
            }
        }
        else
            Snackbar.Add(Localizer["InvalidForm"], Severity.Warning);
    }

    private async Task<bool> ValidateCaptcha(string recaptchaResponse)
    {
        var secretKey = "6LfCwL8qAAAAAMMPjvFljaGn8iMO7Jb6zSWN9Go3";
        var response = await HttpClient.PostAsJsonAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}", new { });
        var captchaResult = await response.Content.ReadFromJsonAsync<CaptchaResult>();
        return captchaResult.Success;
    }

    private void ShowPassword()
    {
        if (_isShow)
        {
            _isShow = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _isShow = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}