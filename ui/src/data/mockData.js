// Mock data for the property booking application
// This file contains sample properties with images, descriptions, amenities, etc.

export const mockProperties = [
  {
    id: 1,
    name: "Luxury Beachfront Villa",
    description: "Experience paradise in this stunning beachfront villa with panoramic ocean views. This spacious 3-bedroom villa features a private infinity pool, direct beach access, and luxurious amenities throughout. Perfect for a family vacation or a romantic getaway, you'll enjoy the tranquility of the ocean while being just a short drive from local attractions and dining options.",
    location: "Malibu, California",
    type: "villa",
    bedrooms: 3,
    bathrooms: 2,
    maxGuests: 6,
    pricePerNight: 399,
    rating: 4.9,
    reviewCount: 48,
    images: [
      "https://images.unsplash.com/photo-1566073771259-6a8506099945?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1512918728675-ed5a9ecdebfd?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "Private Pool",
      "Beachfront",
      "Free WiFi",
      "Kitchen",
      "Air Conditioning",
      "Free Parking",
      "TV",
      "Washer & Dryer",
      "Outdoor Grill",
      "Ocean View"
    ],
    rules: [
      "No pets allowed",
      "No smoking",
      "No parties or events"
    ],
    host: {
      name: "Emily Johnson",
      rating: 4.9,
      responseRate: 99
    }
  },
  {
    id: 2,
    name: "Mountain Retreat Cabin",
    description: "Escape to this charming cabin nestled in the mountains. Surrounded by pine trees and stunning views, this cozy 2-bedroom cabin offers the perfect retreat for nature lovers. Featuring a wood-burning fireplace, hot tub, and spacious deck, it's ideal for hiking adventures during the day and stargazing at night. The fully equipped kitchen and comfortable living space make this your perfect home away from home.",
    location: "Aspen, Colorado",
    type: "cabin",
    bedrooms: 2,
    bathrooms: 1,
    maxGuests: 4,
    pricePerNight: 199,
    rating: 4.8,
    reviewCount: 36,
    images: [
      "https://images.unsplash.com/photo-1518732714860-b62714ce0c59?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1470770841072-f978cf4d019e?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1465572089651-8fde36c892dd?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1484301548518-d0e0a5db0fc8?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "Hot Tub",
      "Fireplace",
      "Mountain View",
      "Free WiFi",
      "Kitchen",
      "Heating",
      "Free Parking",
      "TV",
      "BBQ Grill",
      "Hiking Trails Nearby"
    ],
    rules: [
      "Pets allowed",
      "No smoking indoors",
      "Quiet hours after 10 PM"
    ],
    host: {
      name: "Michael Brown",
      rating: 4.7,
      responseRate: 95
    }
  },
  {
    id: 3,
    name: "Modern Downtown Apartment",
    description: "Stay in the heart of the city in this stylish and modern apartment. This 1-bedroom apartment features contemporary furniture, high ceilings, and large windows that fill the space with natural light. Located in the downtown district, you'll be steps away from top restaurants, shopping, and entertainment venues. The building offers a fitness center, rooftop lounge, and 24-hour security for your convenience and peace of mind.",
    location: "New York, New York",
    type: "apartment",
    bedrooms: 1,
    bathrooms: 1,
    maxGuests: 2,
    pricePerNight: 249,
    rating: 4.6,
    reviewCount: 72,
    images: [
      "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1484154218962-a197022b5858?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "City View",
      "Free WiFi",
      "Kitchen",
      "Air Conditioning",
      "Heating",
      "Elevator",
      "TV",
      "Washer & Dryer",
      "Gym Access",
      "Doorman"
    ],
    rules: [
      "No pets",
      "No smoking",
      "No parties or events",
      "Check-in after 3:00 PM"
    ],
    host: {
      name: "Sophia Wilson",
      rating: 4.9,
      responseRate: 100
    }
  },
  {
    id: 4,
    name: "Charming Historic Townhouse",
    description: "Step back in time in this beautifully restored historic townhouse. Combining original architectural details with modern comforts, this 3-bedroom home offers a unique and charming stay. Enjoy the private garden patio, gourmet kitchen, and cozy living spaces filled with character. Located in a quiet historic district, you'll be within walking distance to local cafes, boutiques, and parks while experiencing the authentic charm of the area.",
    location: "Charleston, South Carolina",
    type: "house",
    bedrooms: 3,
    bathrooms: 2.5,
    maxGuests: 6,
    pricePerNight: 289,
    rating: 4.9,
    reviewCount: 41,
    images: [
      "https://images.unsplash.com/photo-1577495508048-b635879837f1?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1507089947368-19c1da9775ae?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1484301548518-d0e0a5db0fc8?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1597430052043-a9c085b9a908?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "Garden Patio",
      "Free WiFi",
      "Full Kitchen",
      "Air Conditioning",
      "Heating",
      "Washer & Dryer",
      "TV",
      "Fireplace",
      "Free Street Parking",
      "Historic Architecture"
    ],
    rules: [
      "No pets",
      "No smoking",
      "No parties or events",
      "Quiet neighborhood - respect neighbors"
    ],
    host: {
      name: "Robert Davis",
      rating: 4.8,
      responseRate: 98
    }
  },
  {
    id: 5,
    name: "Lakefront Cottage with Dock",
    description: "Relax and unwind at this peaceful lakefront cottage. This 2-bedroom property offers stunning lake views and direct water access with a private dock. Spend your days fishing, swimming, or boating on the lake, and your evenings roasting marshmallows at the firepit or stargazing from the spacious deck. The open floor plan, large windows, and rustic decor create a warm and inviting atmosphere for your lake getaway.",
    location: "Lake Tahoe, California",
    type: "house",
    bedrooms: 2,
    bathrooms: 1,
    maxGuests: 5,
    pricePerNight: 219,
    rating: 4.7,
    reviewCount: 29,
    images: [
      "https://images.unsplash.com/photo-1475113548554-5a36f1f523d6?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1470770841072-f978cf4d019e?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1468824357306-a439d58ccb1c?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1527484500049-478e6a5d0a2d?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "Lakefront",
      "Private Dock",
      "Free WiFi",
      "Kitchen",
      "Air Conditioning",
      "Heating",
      "Fire Pit",
      "Deck",
      "BBQ Grill",
      "Canoe included"
    ],
    rules: [
      "Pets allowed with fee",
      "No smoking indoors",
      "Life jackets must be worn on the dock",
      "No loud noises after 9 PM"
    ],
    host: {
      name: "Jennifer Miller",
      rating: 4.9,
      responseRate: 97
    }
  },
  {
    id: 6,
    name: "Luxury Penthouse with City Views",
    description: "Indulge in luxury in this spectacular penthouse apartment with panoramic city views. This premium 3-bedroom penthouse offers high-end furnishings, floor-to-ceiling windows, and a private terrace for enjoying the breathtaking skyline. The gourmet kitchen, spa-like bathrooms, and spacious living areas make this an ideal choice for those seeking a sophisticated urban retreat. Building amenities include a rooftop pool, fitness center, and concierge service.",
    location: "Miami, Florida",
    type: "apartment",
    bedrooms: 3,
    bathrooms: 3,
    maxGuests: 6,
    pricePerNight: 499,
    rating: 4.9,
    reviewCount: 18,
    images: [
      "https://images.unsplash.com/photo-1558682775-94197f4d0f88?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1546555648-fb7876c40c58?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "City View",
      "Private Terrace",
      "Free WiFi",
      "Gourmet Kitchen",
      "Air Conditioning",
      "Heating",
      "Pool Access",
      "Gym Access",
      "Concierge Service",
      "Smart Home Features"
    ],
    rules: [
      "No pets",
      "No smoking",
      "No parties or events",
      "Age restriction: 25+ years"
    ],
    host: {
      name: "Alexander White",
      rating: 5.0,
      responseRate: 100
    }
  },
  {
    id: 7,
    name: "Cozy Studio in Historic District",
    description: "Experience city living in this stylish studio apartment located in the heart of the historic district. This thoughtfully designed space features modern furnishings, a well-equipped kitchenette, and large windows offering views of the charming neighborhood. The perfect home base for exploring the city, you'll be steps away from historic sites, trendy restaurants, and vibrant nightlife. Ideal for solo travelers or couples seeking a convenient and comfortable urban retreat.",
    location: "Boston, Massachusetts",
    type: "apartment",
    bedrooms: 0,
    bathrooms: 1,
    maxGuests: 2,
    pricePerNight: 149,
    rating: 4.7,
    reviewCount: 65,
    images: [
      "https://images.unsplash.com/photo-1556912167-f556f1f39ffa?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1540518614846-7eded433c457?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1513694203232-719a280e022f?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "City View",
      "Free WiFi",
      "Kitchenette",
      "Air Conditioning",
      "Heating",
      "TV",
      "Elevator",
      "Washer & Dryer in Building",
      "Coffee Maker",
      "Keyless Entry"
    ],
    rules: [
      "No pets",
      "No smoking",
      "No parties or events",
      "Not suitable for children"
    ],
    host: {
      name: "David Chen",
      rating: 4.8,
      responseRate: 95
    }
  },
  {
    id: 8,
    name: "Secluded Desert Oasis",
    description: "Find peace and tranquility at this unique desert property. This architectural gem offers 2 bedrooms, an open concept living area, and stunning views of the desert landscape through expansive windows. The private pool and hot tub are perfect for relaxing after a day of exploring nearby national parks. The property's thoughtful design seamlessly blends indoor and outdoor living, allowing you to experience the beauty of the desert in complete comfort.",
    location: "Joshua Tree, California",
    type: "house",
    bedrooms: 2,
    bathrooms: 2,
    maxGuests: 4,
    pricePerNight: 325,
    rating: 4.9,
    reviewCount: 27,
    images: [
      "https://images.unsplash.com/photo-1518780664697-55e3ad937233?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1464146072230-91cabc968266?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1523217582562-09d0def993a6?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      "https://images.unsplash.com/photo-1493809842364-78817add7ffb?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
    ],
    amenities: [
      "Private Pool",
      "Hot Tub",
      "Desert View",
      "Free WiFi",
      "Full Kitchen",
      "Air Conditioning",
      "Heating",
      "Outdoor Shower",
      "Stargazing Deck",
      "Fire Pit"
    ],
    rules: [
      "No pets",
      "No smoking",
      "No parties or events",
      "Conservation area - respect nature"
    ],
    host: {
      name: "Maria Garcia",
      rating: 5.0,
      responseRate: 100
    }
  }
];