document.addEventListener('DOMContentLoaded', () => {
    const settingsForm = document.getElementById('settings-form');
    const avatarUpload = document.getElementById('avatar-upload');
    const avatarPreview = document.getElementById('avatar-preview');
    const darkModeToggle = document.getElementById('dark-mode-toggle');
    const snackbar = document.getElementById('snackbar');

    // Avatar Preview
    avatarUpload.addEventListener('change', function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = (e) => {
                avatarPreview.style.backgroundImage = `url(${e.target.result})`;
            }
            reader.readAsDataURL(file);
        }
    });

    // Dark Mode
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const savedTheme = localStorage.getItem('theme');

    if (savedTheme === 'dark' || (!savedTheme && prefersDark)) {
        document.documentElement.classList.add('dark-mode');
        darkModeToggle.checked = true;
    }

    darkModeToggle.addEventListener('change', () => {
        if (darkModeToggle.checked) {
            document.documentElement.classList.add('dark-mode');
            localStorage.setItem('theme', 'dark');
        } else {
            document.documentElement.classList.remove('dark-mode');
            localStorage.setItem('theme', 'light');
        }
    });

    // Form Submission
    settingsForm.addEventListener('submit', (e) => {
        e.preventDefault();
        // In a real app, you would send this data to a server.
        // For now, we'll just show a confirmation.

        // Example of getting data:
        const formData = {
            displayName: document.getElementById('display-name').value,
            about: document.getElementById('about-me').value,
            language: document.getElementById('language').value,
            darkMode: darkModeToggle.checked,
            showAgeGender: document.getElementById('show-age-gender').checked,
        };

        console.log('Saving settings:', formData);

        // Store in localStorage for demonstration
        localStorage.setItem('userSettings', JSON.stringify(formData));

        showSnackbar('Settings saved successfully!');
    });

    function showSnackbar(message) {
        snackbar.textContent = message;
        snackbar.classList.add('show');
        setTimeout(() => {
            snackbar.classList.remove('show');
        }, 3000);
    }
});