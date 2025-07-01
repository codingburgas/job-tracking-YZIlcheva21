import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-user',
  imports: [RouterLink],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {
  firstName: string = '';
  middleName: string = '';
  lastName: string = '';
  username: string = '';
  role: string = "user";
}