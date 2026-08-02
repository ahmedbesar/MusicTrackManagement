import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  username = '';
  password = '';

  readonly submitting = signal(false);
  readonly error = signal<string | null>(null);

  onSubmit(): void {
    this.submitting.set(true);
    this.error.set(null);

    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        this.submitting.set(false);
        this.router.navigateByUrl('/tracks');
      },
      error: (response) => {
        this.submitting.set(false);
        this.error.set(
          response?.status === 401 ? 'Invalid username or password.' : 'Failed to sign in. Please try again.'
        );
      }
    });
  }
}
