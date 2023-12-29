import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  showSpinner = false;
 username: string = '';
  @Input() password: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  login() : void {
      this.authService.login(this.username, this.password).subscribe(
        success => {
          
          this.router.navigate(['/']);
        },
        error => {
          alert('Incorrect login info!')
        }
      );
    }  
}
