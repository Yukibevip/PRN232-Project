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
            const targetForm = document.getElementById(`${target}-form`);
            if (targetForm) targetForm.classList.add('active');
        });
    });

    // Form validation and submission (validate client-side then allow normal POST)
    const signInForm = document.getElementById('signin-form');
    const signUpForm = document.getElementById('signup-form');

    if (signInForm) {
        signInForm.addEventListener('submit', function (e) {
            e.preventDefault();
            if (validateForm(this)) {
                // allow server-side handling
                this.submit();
            } else {
                showToast('Please fix the highlighted fields.', 'error');
            }
        });
    }

    if (signUpForm) {
        signUpForm.addEventListener('submit', function (e) {
            e.preventDefault();
            if (validateForm(this)) {
                // allow server-side handling
                this.submit();
            } else {
                showToast('Please fix the highlighted fields.', 'error');
            }
        });
    }

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
            if (password && confirmPassword && password.value !== confirmPassword.value) {
                showError(confirmPassword, 'Passwords do not match.');
                isValid = false;
            } else if (confirmPassword) {
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
        if (input.type === 'email' && input.value && !/\S+@\S+\.\S+/.test(input.value)) {
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

    // Toast functionality (guard when toast not present)
    const toastElement = document.getElementById('toast');
    function showToast(message, type = 'success') {
        if (!toastElement) return;
        toastElement.textContent = message;
        toastElement.style.display = 'block';
        toastElement.className = 'toast show';
        toastElement.classList.add(type);

        setTimeout(() => {
            toastElement.className = 'toast';
            toastElement.style.display = 'none';
        }, 3000);
    }
});