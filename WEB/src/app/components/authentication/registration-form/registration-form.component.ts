import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-registration-form',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.scss'],
})
export class RegistrationFormComponent {
  registerForm: FormGroup;
  showPassword = false;
  registrationError: string | null = null;
  registrationSuccess: string | null = null;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    // Initialize form with controls and validators
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(64), this.nameValidator()]],
      middleName: ['', [Validators.required, Validators.maxLength(64), this.nameValidator()]],
      lastName: ['', [Validators.required, Validators.maxLength(64), this.nameValidator()]],
      username: ['', [Validators.required, Validators.maxLength(32), this.usernameValidator()]],
      password: ['', [Validators.required, Validators.maxLength(128), this.passwordComplexityValidator()]],
      confirmPassword: ['', Validators.required],
      role: ['user'],
    }, { validators: this.passwordsMatch });
  }

  toggleShowPassword() {
    this.showPassword = !this.showPassword;
  }

  // Validates names: letters only, min length 2, at least one uppercase letter
  nameValidator(): ValidatorFn {
    const lettersOnly = /^[A-Za-z]+$/;
    return (control: AbstractControl): ValidationErrors | null => {
      const val = control.value;
      if (!val) return null;
      if (!lettersOnly.test(val)) return { pattern: true };
      if (val.length < 2) return { minlength: { requiredLength: 2, actualLength: val.length } };
      if (!/[A-Z]/.test(val)) return { capitalLetter: true };
      return null;
    };
  }

  // Optional name validator: allows empty, otherwise same checks as nameValidator
  optionalNameValidator(): ValidatorFn {
    const lettersOnly = /^[A-Za-z]*$/;
    return (control: AbstractControl): ValidationErrors | null => {
      const val = control.value;
      if (!val) return null;
      if (!lettersOnly.test(val)) return { pattern: true };
      if (val.length > 0 && val.length < 2) return { minlength: { requiredLength: 2, actualLength: val.length } };
      if (val.length > 0 && !/[A-Z]/.test(val)) return { capitalLetter: true };
      return null;
    };
  }

  // Username validator: minimum length 3
  usernameValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const val = control.value;
      if (!val) return null;
      return val.length >= 3 ? null : { minlength: { requiredLength: 3, actualLength: val.length } };
    };
  }

  // Password complexity validator
  passwordComplexityValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const val: string = control.value || '';
      if (!val) return null;

      const errors: ValidationErrors = {};
      if (val.length < 8) errors['minlength'] = { requiredLength: 8, actualLength: val.length };
      if (!/[A-Z]/.test(val)) errors['uppercase'] = true;
      if (!/[a-z]/.test(val)) errors['lowercase'] = true;
      if (!/\d/.test(val)) errors['number'] = true;
      if (!/[!@#$%^&*\~\?]/.test(val)) errors['specialChar'] = true;

      return Object.keys(errors).length ? errors : null;
    };
  }

  // Form-level validator to check matching passwords
  passwordsMatch(group: AbstractControl): ValidationErrors | null {
    const pass = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return pass === confirm ? null : { passwordMismatch: true };
  }

  onSubmit() {
    this.registrationError = null;
    this.registrationSuccess = null;

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();

      // Scroll to first invalid control
      const firstInvalidControl = Object.keys(this.registerForm.controls)
        .map(key => ({ key, control: this.registerForm.get(key) }))
        .find(entry => entry.control && entry.control.invalid);

      if (firstInvalidControl) {
        const el = document.querySelector(`[formControlName="${firstInvalidControl.key}"]`);
        if (el) (el as HTMLElement).scrollIntoView({ behavior: 'smooth', block: 'center' });
      }
      return;
    }

    this.isSubmitting = true;

    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.registrationSuccess = 'Успешна регистрация!';
        this.registerForm.reset();

        setTimeout(() => {
          this.registrationSuccess = null;
          this.router.navigate(['/login-form']);
        }, 3000);
      },
      error: (err) => {
        this.isSubmitting = false;

        if (
          err.status === 409 ||
          err.error?.message?.includes('already exists') ||
          err.error?.message?.toLowerCase().includes('вече съществува')
        ) {
          this.registrationError = 'Потребителското име вече съществува.';
        } else if (err.status === 500) {
          this.registrationError = 'Сървърна грешка. Моля, опитайте отново по-късно.';
        } else {
          this.registrationError = 'Възникна грешка при регистрацията.';
        }

        setTimeout(() => (this.registrationError = null), 5000);
      },
    });
  }
}
