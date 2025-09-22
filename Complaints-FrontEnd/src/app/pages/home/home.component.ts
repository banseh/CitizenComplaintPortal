import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { SearchService } from '../../services/Search/search.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
  ],
  providers: [SearchService]
})
export class HomeComponent {
constructor(private router: Router, private searchService: SearchService) {}
  showComplaintPopup: boolean = false;
  status: string = '';
  showNotFoundPopup = false;
  showSearchPopup = false;
  categoryFromCard: string = '';
  searchData = {
    email: '',
    ticketNumber: ''
  };
  foundComplaint: any;
emailDisabled: boolean = false;
  issues = [
    {
      title: 'Electricity',
      image: 'Electricity.png',
      description: 'Report power outages, voltage problems, damaged meters, or unsafe wiring.'
    },
    {
      title: 'Water',
      image: 'Water.png',
      description: 'Report water cuts, low pressure, or polluted water.'
    },
    {
      title: 'Gas',
      image: 'Gas.png',
      description: 'Report gas leaks, meter issues, or interruptions in gas supply.'
    },
    {
      title: 'Waste Management',
      image: 'WM.png',
      description: 'Report uncollected garbage, overflowing bins, or sewage problems.'
    }
  ];

  isLoading = false;

  
    ngDoCheck() {
      this.checkEmail(); 
    }

  checkEmail() {
  const email = localStorage.getItem('email');

  if (!email) {
    this.emailDisabled = false;
    this.searchData.email = this.searchData.email;
  } else if (!this.searchData.email) {
    this.searchData.email = email;
    this.emailDisabled = true;
  }
}


  
  submitComplaint(): void {
    this.router.navigate(['/submitComplaint']);
  }

  goToComplaintForm(categoryName: string): void {
    this.router.navigate(['/complaint-category'], {
      queryParams: { category: categoryName }
    });
  }

  onSearchSubmit(form: NgForm): void {
    this.isLoading = true;
    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }

    console.log( this.searchData);
    this.searchService.getCaseByTicketNumberAndEmail(
      this.searchData.ticketNumber,
      this.searchData.email
    ).subscribe({
      next: (res) => {
        this.isLoading = false;
        if(res.status== 404)
        {
          this.showSearchPopup = true;
        }
        this.foundComplaint = res;
        this.showSearchPopup = false;
        console.log('Complaint found: ', res);
        this.showComplaintPopup = true;
        this.foundComplaint = res.data;
        if(res.data.statusCode == 121570001) { this.status="New"}
        else if(res.data.statusCode == 1){this.status="Pending"}
        else if(res.data.statusCode == 2){this.status="In Progress"}
         else if(res.data.statusCode == 3){this.status="Resolved"}
        console.log("res.data.statusCode: ",res.data.statusCode);
        console.log("this.status: ",this.status)
      },
      error: (err) => {
        this.isLoading = false;
        this.showSearchPopup = false;
        this.showNotFoundPopup= true;
        setTimeout(() => {this.showNotFoundPopup= false;}, 1000);
      }
    });
  }
  closePopupAndResetForm() {
    this.showComplaintPopup = false;
    this.searchData.email=" ";
    this.searchData.ticketNumber=" " ;
  }
}
