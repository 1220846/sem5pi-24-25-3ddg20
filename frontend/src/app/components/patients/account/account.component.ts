import { Component, Input, OnInit } from '@angular/core';
import { User } from '@auth0/auth0-angular';
import { DeleteAccountComponent } from './delete-account/delete-account.component';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [DeleteAccountComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss'
})

export class AccountComponent implements OnInit {
  user: User | null = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.loggedInUser().subscribe(user => {
      this.user = user;
    });
  }
}
