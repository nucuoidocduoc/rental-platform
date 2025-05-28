import { Link } from 'react-router-dom';

const Navbar = () => {
  return (
    <nav className="bg-white shadow-md">
      <div className="container mx-auto px-4 py-3">
        <div className="flex justify-between items-center">
          <Link to="/" className="text-2xl font-bold text-blue-600">
            PropertyBooker
          </Link>
          
          <div className="flex space-x-6">
            <Link to="/" className="text-gray-700 hover:text-blue-600 font-medium">
              Home
            </Link>
            <Link to="/properties" className="text-gray-700 hover:text-blue-600 font-medium">
              Properties
            </Link>
            <a href="#" className="text-gray-700 hover:text-blue-600 font-medium">
              About
            </a>
            <a href="#" className="text-gray-700 hover:text-blue-600 font-medium">
              Contact
            </a>
          </div>
          
          <div className="flex items-center space-x-4">
            <button className="bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded-md transition">
              Sign In
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;