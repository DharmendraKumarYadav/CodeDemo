import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-auth-layout',
  templateUrl: './auth-layout.component.html',
  styleUrls: ['./auth-layout.component.scss']
})
export class AuthLayoutComponent implements OnInit {
  logo= require('../../../../assets/logo/logo.png')
  constructor() { }

  ngOnInit(): void {

    
  }

}
