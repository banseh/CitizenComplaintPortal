import { Component, Input } from '@angular/core';
import { CategoryService } from '../../services/Category/category.service';
import { ComplaintService } from '../../services/Complaint/complaint.service';
import { ContactServeiceService } from '../../services/Contact/contact-serveice.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-complaint-category',
  imports: [CommonModule, FormsModule],
  templateUrl: './complaint-category.component.html',
  styleUrl: './complaint-category.component.css'
})
export class ComplaintCategoryComponent {
  category: string = '';

  complaint = {
    categoryBinding: '',
    name: '',
    description: '',
    nationalId: '',
    email: '',
    location: ''
  }
  user = {
    firstname: '',
    lastname: '',
    email: '',
    gender: '',
    isAnynoums: ''
  };
  isSubmitting: boolean = false;
  showSuccessPopup = false;
  showErrorPopup = false;
  TiketNumber:string=" "
  @Input() selectedCategory: string = '';
  selectedValue: string = '';
  selectedCategoryId: string = '';
  categories: any[] = [];

  constructor(private categoryService: CategoryService ,private complaintService: ComplaintService,private contactServeiceService:ContactServeiceService , private router:Router , private route: ActivatedRoute) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    this.route.queryParams.subscribe(params => {
      const categoryName = params['category'];
      if (categoryName) {
        this.categoryService.getAllCategories().subscribe({
          next: (res) => {
            this.categories = res.data;
            const matchedCategory = this.categories.find(cat => cat.name.toLowerCase() === categoryName.toLowerCase());
            if (matchedCategory) {
              this.selectedCategoryId = matchedCategory.categoryId;
              this.complaint.categoryBinding = `/categories(${matchedCategory.categoryId})`;
            }
          },
          error: (err) => {
            console.error('Error fetching categories:', err);
          }
        });
      } else {
        this.categoryService.getAllCategories().subscribe({
          next: (res) => {
            this.categories = res.data;
          },
          error: (err) => {
            console.error('Error fetching categories:', err);
          }
        });
      }
    });

    if (token) {
      this.contactServeiceService.getUserByToken().subscribe({
        next: (res) => {
          this.user.email = res.email;
          this.user.firstname = res.firstname;
          this.user.gender = res.gender;
          this.user.lastname = res.lastname;
          this.user.isAnynoums = res.isAnynoums;
        },
        error: (err) => {
          console.error('Error fetching User Info:', err);
        }
      });
    }
  }

  get isPlaceholderSelected(): boolean {
    return !this.complaint.categoryBinding;
  }

  submitComplaint(form: any) {
    if (form.invalid) {
    form.control.markAllAsTouched();
    return;
  }
  this.isSubmitting = true;

  this. complaint = {
    categoryBinding: `/categories(${this.selectedCategoryId})`,
    name: this.complaint.name,
    description:  this.complaint.description,
    nationalId:this.complaint.nationalId,
    email: this.complaint.email,
    location:this.complaint.location,
  };

  console.log('Complaint submitted: ', this.complaint);
  const token = localStorage.getItem('token');
  if(token){ }
  else
  {
    this.complaintService.submitGuestComplaint(this.complaint).subscribe({
    next: (res) => {
      console.log('Complaint submitted guest successfully: ', res);
      if (res.status) {
        this.TiketNumber= res.data;
        console.log("this.TiketNumber: ", this.TiketNumber )
        this.isSubmitting = false;
        this.showSuccessPopup = true;
      }
        else{
        this.isSubmitting = false;
        this.showErrorPopup = true;
        setTimeout(() => this.showErrorPopup = false, 1000);
      }
      },
      error: (err) => {
        console.error('Failed to submit complaint:', err);
          this.isSubmitting = false;
          this.showErrorPopup = true;
          setTimeout(() => this.showErrorPopup = false, 1000);
          console.error('Complaint submission failed:', err);
        }
    });
  }
    form.resetForm();
  }


  onCategoryChange() {
    console.log('Selected categoryId:', this.selectedCategoryId);
  }
  closePopups() {
    this.showSuccessPopup = false;
    this.showErrorPopup = false;
  }
  pressme() {
    this.showSuccessPopup = false;
    this.router.navigate(['/home']);
  }
}
