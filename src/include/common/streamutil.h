#ifndef CHALLENGES_STREAMUTILS_HPP
#define CHALLENGES_STREAMUTILS_HPP

#include <string>
#include <vector>
#include <istream>

namespace challenges {

    std::vector<std::string> read_lines(std::istream & is, int count = 1);

    std::vector<int> read_ints(std::istream & is);
}

#endif //CHALLENGES_STREAMUTILS_HPP
