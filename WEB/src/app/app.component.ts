import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NavBarComponent } from './components/navigation/nav-bar/nav-bar.component';

import { AuthService } from './services/auth.service';

@Component({
    selector: 'app-root',
    imports: [CommonModule, RouterOutlet, RouterModule, TranslateModule, NavBarComponent],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss',
    standalone: true,
})
export class AppComponent {
  title = 'job-training';
  constructor(public auth: AuthService) {}
}
