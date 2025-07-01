import { RouterModule, Routes } from '@angular/router';
import { JobAdComponent } from './components/job-ad/job-ad.component';
import { JobApplicationComponent } from './components/job-application/job-application.component';
import { RegistrationFormComponent } from './components/authentication/registration-form/registration-form.component';
import { LoginFormComponent } from './components/authentication/login-form/login-form.component';
import { AppComponent } from './app.component';
import { AuthGuard } from './guards/auth.guard';
import { ProfileComponent } from './components/profile/profile.component';
import { CreateJobAdComponent } from './components/create-job/create-job-ad.component';
import { AdminGuard } from './guards/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/job-ads', pathMatch: 'full' },
  { path: 'login-form', component: LoginFormComponent },
  { path: 'registration-form', component: RegistrationFormComponent },

  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'job-ads', component: JobAdComponent, canActivate: [AuthGuard] },
  { path: 'job-applications', component: JobApplicationComponent, canActivate: [AuthGuard] },
  { path: 'create-job-ad', component: CreateJobAdComponent, canActivate: [AuthGuard, AdminGuard] },

  { path: '**', redirectTo: 'login-form' }
];