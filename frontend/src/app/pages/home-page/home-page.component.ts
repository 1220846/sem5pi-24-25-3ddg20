import { Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  constructor(private auth: AuthService) {} 

  login() {
    this.auth.loginWithRedirect();
  }

  register() {
    const redirectUri = `${window.location.origin}/callback`; 
    window.location.href = `https://YOUR_DOMAIN.auth0.com/authorize?response_type=token&client_id=YOUR_CLIENT_ID&redirect_uri=${redirectUri}&screen_hint=signup`;
  }
}