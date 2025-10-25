document.addEventListener('DOMContentLoaded', () => {
    const tabs = document.querySelectorAll('.tab');
    const formSections = document.querySelectorAll('.form-section');

    // Tab switching logic
    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            const target = tab.dataset.tab;

            tabs.forEach(t => {
                t.classList.remove('active');
                t.setAttribute('aria-selected', 'false');
            });
            tab.classList.add('active');
            tab.setAttribute('aria-selected', 'true');

            formSections.forEach(form => {
                form.classList.remove('active');
            });
            document.getElementById(`${target}-form`).classList.add('active');
        });
    });

    // Form validation and submission
    const signInForm = document.getElementById('signin-form');
    const signUpForm = document.getElementById('signup-form');

    signInForm.addEventListener('submit', function (e) {
        e.preventDefault();
        if (validateForm(this)) {
            // Mock successful login
            showToast('Login Successful!', 'success');
            setTimeout(() => {
                window.location.href = 'call.html';
            }, 1500);
        }
    });

    signUpForm.addEventListener('submit', function (e) {
        e.preventDefault();
        if (validateForm(this)) {
            // Mock successful sign up
            showToast('Account Created Successfully!', 'success');
            setTimeout(() => {
                window.location.href = 'call.html';
            }, 1500);
        }
    });

    function validateForm(form) {
        let isValid = true;
        const inputs = form.querySelectorAll('input[required]');

        inputs.forEach(input => {
            if (!validateInput(input)) {
                isValid = false;
            }
        });

        // Specific validation for sign up form passwords
        if (form.id === 'signup-form') {
            const password = form.querySelector('#signup-password');
            const confirmPassword = form.querySelector('#signup-confirm-password');
            if (password.value !== confirmPassword.value) {
                showError(confirmPassword, 'Passwords do not match.');
                isValid = false;
            } else {
                hideError(confirmPassword);
            }
        }

        return isValid;
    }

    function validateInput(input) {
        let valid = true;
        const errorElement = input.nextElementSibling;

        // Reset
        hideError(input);

        // Required
        if (input.required && input.value.trim() === '') {
            showError(input, 'This field is required.');
            valid = false;
        }

        // Email
        if (input.type === 'email' && !/\S+@\S+\.\S+/.test(input.value)) {
            showError(input, 'Please enter a valid email.');
            valid = false;
        }

        // Password length
        if (input.type === 'password' && input.hasAttribute('minlength') && input.value.length < input.getAttribute('minlength')) {
            showError(input, `Password must be at least ${input.getAttribute('minlength')} characters.`);
            valid = false;
        }

        return valid;
    }

    function showError(input, message) {
        const errorElement = input.nextElementSibling;
        input.setAttribute('aria-invalid', 'true');
        if (errorElement && errorElement.classList.contains('error-message')) {
            errorElement.textContent = message;
            errorElement.style.display = 'block';
        }
    }

    function hideError(input) {
        const errorElement = input.nextElementSibling;
        input.setAttribute('aria-invalid', 'false');
        if (errorElement && errorElement.classList.contains('error-message')) {
            errorElement.style.display = 'none';
        }
    }

    // Toast functionality
    const toastElement = document.getElementById('toast');
    function showToast(message, type = 'success') {
        toastElement.textContent = message;
        toastElement.className = 'toast show';
        toastElement.classList.add(type);

        setTimeout(() => {
            toastElement.className = 'toast';
        }, 3000);
    }
});