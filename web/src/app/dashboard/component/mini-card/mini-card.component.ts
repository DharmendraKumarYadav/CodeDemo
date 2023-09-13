import { Component, Input, OnInit } from '@angular/core';
@Component({
  selector: 'app-mini-card',
  templateUrl: './mini-card.component.html',
  styleUrls: ['./mini-card.component.scss']
})
export class MiniCardComponent implements OnInit {
  @Input() title: string;
  @Input() count: number;
  @Input() icon: string;
  @Input() text: string;
  @Input() colour:string;
  constructor(){}

  ngOnInit(): void {
   
  }

}