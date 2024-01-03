import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { routesEnum } from 'src/app/enumerators/routesEnum';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private readonly router: Router) { }

  get isLoginPage(): boolean {
    return this.router.url === '/' + routesEnum.login.url;
  }

  ngOnInit(): void {
  }
    
  logoff() : void {
    this.router.navigate([routesEnum.login.url]);
  }  
}
