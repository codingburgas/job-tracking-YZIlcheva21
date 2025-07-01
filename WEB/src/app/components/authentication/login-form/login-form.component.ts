// Angular core and common imports
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

// Services
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login-form',
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss',
  standalone: true
})
export class LoginFormComponent {
  // Form group for login inputs
  loginForm: FormGroup;

  // UI state
  showPassword = false;
  errorMessage: string | null = null;
  loginError = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    // Initialize form with validators
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  // Handle login form submission
  onLogin() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;

      this.authService.login(username, password).subscribe({
        next: (response) => {
          console.log('Login success', response);
          this.loginError = false;
          localStorage.setItem('jwt', response.token);
          this.router.navigate(['job-ads']);
        },
        error: (err) => {
          console.error('Login error', err);
          this.loginError = true;
        }
      });
    }
  }

  // Toggle visibility of password input
  toggleShowPassword() {
    this.showPassword = !this.showPassword;
  }
}