import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JobAdsService } from '../../services/job-ads.service';

@Component({
  selector: 'app-create-job-ad',
  imports: [CommonModule, FormsModule],
  templateUrl: './create-job-ad.component.html',
  styleUrls: ['./create-job-ad.component.scss'],
  standalone: true
})
export class CreateJobAdComponent {

  jobAd = {
    title: '',
    companyName: '',
    description: '',
    publishedOn: new Date().toISOString().split('T')[0], 
    isOpen: true,
  };

  alertMessage: string | null = null; 
  alertType: 'success' | 'danger' | null = null;
  isSubmitting = false;

  constructor(private jobAdsService: JobAdsService, private router: Router) {}

  showAlert(message: string, type: 'success' | 'danger') {
    this.alertMessage = message;
    this.alertType = type;
    setTimeout(() => {
      this.alertMessage = null;
      this.alertType = null;
    }, 2000);
  }

  onSubmit() {
    if (this.isSubmitting) {
      return;
    }
    
    this.isSubmitting = true;

    this.jobAdsService.createJobAd(this.jobAd).subscribe({
      next: () => {
        setTimeout(() => {
          this.router.navigate(['/job-ads']);
        }, 1500);
      },
      error: (err) => {
        console.error(err);
        this.showAlert('Възникна грешка при добавянето на обявата.', 'danger');
        this.isSubmitting = false;
      },
      complete: () => {
        this.isSubmitting = false;
      }
    });
  }
}