import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-footer',
  imports: [RouterLink,CommonModule],
  templateUrl: './footer.component.html',
})



export class FooterComponent {
  constructor( private router: Router) {}
  quickLinks = [
    { title: "Home", href: "/" },
    { title: "Submit Complaint", href: "/submitComplaint" },
  ];

  popularCategories = [
    { title: "Electricity"},
    { title: "Water" },
    { title: "Gas"},
    { title: "Waste Management"},
  ];

  contactInfo = [
    { text: "Phone: +123 456 7890", href: "tel:+1234567890" },
    { text: "Email: support@learnify.com", href: "mailto:support@learnify.com" },
    { text: "Address:\n1901 Thornridge Cir.\nShiloh, Hawaii 81063", href: "#" },
  ];

  submitComplaint(){
  this.router.navigate(["/submitComplaint"])
  }

  About(){
    this.router.navigate(["/about"])
  }
}
