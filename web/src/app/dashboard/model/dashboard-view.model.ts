
export interface DashboardModel {
    countModel: CountModel
    monthlyBooking: MonthlyBooking[]
    monthlyCustomer: any[]
  }
  
  export interface CountModel {
    customerCount: number
    saleBikeCount: number
    bookingCount: number
    bikeCount: number
  }
  
  export interface MonthlyBooking {
    monthName: string
    count: number
  }