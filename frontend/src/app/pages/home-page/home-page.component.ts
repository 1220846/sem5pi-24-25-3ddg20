import { Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { CommonModule } from '@angular/common'; // Importação para standalone
import { LoginComponent } from '../../components/authentication/login/login.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule,LoginComponent],
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent {
  constructor() {} 

}
